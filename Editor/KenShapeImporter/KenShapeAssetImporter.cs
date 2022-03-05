using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using BracedFramework;

namespace BracedFramework
{
    [ScriptedImporter(1, "kenshape")]
    public class KenShapeAssetImporter : ScriptedImporter
    {
        public int[] HDRColors = new int[16];
        public int HDRMultiplier = 2;


        [System.Serializable]
        public class KenShapeRootObject
        {
            public string title;
            public string version;
            public KenShapeSize size;
            public float depthMultiplier;
            public int alignment;
            public KenShapeTile[] tiles;
            public string[] colors;

            [System.Serializable]
            public class KenShapeSize
            {
                public int x;
                public int y;

            }

            [System.Serializable]
            public class KenShapeTile
            {
                public int shape;
                public int angle;
                public int color;
                public int depth;
            }
        }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var bytes = File.ReadAllBytes(ctx.assetPath);
            var text = Unzip(bytes);

            var jsonObject = JsonUtility.FromJson<KenShapeRootObject>(text);

            KenShapeModel model = ScriptableObject.CreateInstance<KenShapeModel>();
            model.Size = new Vector3Int(jsonObject.size.x, jsonObject.size.y, 0);
            model.DepthMultiplier = jsonObject.depthMultiplier /100f;

            for (int i = 0; i < jsonObject.tiles.Length; i++)
            {
                var tile = jsonObject.tiles[i];
                if (tile.color == -1)
                    continue;

                // top left is 0,0
                // bottom left is index 16 (for a 16x16)
                var offset = new Vector2(-jsonObject.size.x / 2f + 0.5f, -jsonObject.size.y / 2f + 0.5f);
                float floatSize = 16f;

                var newVox = new Kenxel()
                {
                    Position = new Vector3(
                        -(i / jsonObject.size.y + offset.x) / floatSize,
                        -(i % jsonObject.size.y + offset.y) / floatSize,
                        0f),
                    Depth = tile.depth,
                    ColorIndex = tile.color,
                    //Rotation = 270 + tile.angle * 90f,
                    Rotation = 90 + tile.angle * 90f,
                    Shape = tile.shape,
                    HDRLevel = 0 + HDRColors[tile.color] * HDRMultiplier,
                };

                model.Kenxels.Add(newVox);

                ColorUtility.TryParseHtmlString(jsonObject.colors[tile.color], out newVox.Color);
            }
            var path = "Packages/com.qorthos.bracedframework/Editor/KenShapeImporter/KenShapeMeshDefs.asset";
            KenShapeMeshDef meshDefs = AssetDatabase.LoadAssetAtPath<KenShapeMeshDef>(path);

            var newMesh = new Mesh();

            List<Vector3> vertices = new List<Vector3>();
            List<Color> colors = new List<Color>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> indices = new List<int>();
            
            int indexCount = 0;

            foreach (var vox in model.Kenxels)
            {
                var refMesh = meshDefs.Meshes[vox.Shape];

                var rotation = Quaternion.Euler(new Vector3(0, 0, vox.Rotation));

                foreach (var vertex in refMesh.vertices)
                {
                    var newVertex = rotation * vertex;
                    
                    newVertex += vox.Position;
                    newVertex.z *= 1 + (0.5f * (vox.Depth - 1f) * model.DepthMultiplier);
                    vertices.Add(newVertex);
                    colors.Add(vox.Color);
                    uvs.Add(new Vector2(1 + vox.HDRLevel, 0));
                }

                List<int> newIndices = new List<int>(refMesh.GetIndices(0));
                for (int i = 0; i < newIndices.Count; i++)
                {
                    newIndices[i] += indexCount;
                }

                indexCount += refMesh.vertexCount;

                indices.AddRange(newIndices);
            }

            newMesh.vertices = vertices.ToArray();
            newMesh.uv = uvs.ToArray();
            newMesh.colors = colors.ToArray();
            newMesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
            newMesh.RecalculateBounds();
            newMesh.RecalculateNormals();
            newMesh.RecalculateTangents();


            model.mesh = newMesh;
            newMesh.name = $"{jsonObject.title}Mesh";

            ctx.AddObjectToAsset("kenShapeMesh", newMesh);
            ctx.AddObjectToAsset("kenShapeModel", model);
            ctx.SetMainObject(model);
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))

            using (var mso = new MemoryStream())
            {

                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }

        }
    }
}