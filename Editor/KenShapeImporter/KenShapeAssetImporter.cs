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
        public bool RotateAxis;
        public int[] HDRIntensities = new int[16];

        public bool UseBackfaceColor;
        public Color BackfaceColor;

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
            model.UseBackfaceColor = UseBackfaceColor;
            model.BackfaceColor = BackfaceColor;
            var zOffset = -(jsonObject.alignment - 1) / 32f;

            for (int i = 0; i < jsonObject.tiles.Length; i++)
            {
                var tile = jsonObject.tiles[i];
                if (tile.color == -1)
                    continue;

                // top left is 0,0
                // bottom left is index 16 (for a 16x16)
                var offset = new Vector2(-jsonObject.size.x / 2f + 0.5f, -jsonObject.size.y / 2f + 0.5f);
                float floatSize = 16f;

                var newVox = new KenxelData()
                {
                    Position = new Vector3(
                        -(i / jsonObject.size.y + offset.x) / floatSize,
                        -(i % jsonObject.size.y + offset.y) / floatSize,
                        zOffset),
                    Depth = 1 + ((tile.depth - 1f) * model.DepthMultiplier),
                    ColorIndex = tile.color,
                    //Rotation = 270 + tile.angle * 90f,
                    Rotation = 90 + tile.angle * 90f,
                    Shape = tile.shape,
                    HDRIntensity = 0 + HDRIntensities[tile.color],
                };

                model.Kenxels.Add(newVox);

                ColorUtility.TryParseHtmlString(jsonObject.colors[tile.color], out newVox.Color);
            }

            var newMesh = CreateRawMesh(model);

            model.mesh = newMesh;
            newMesh.name = $"{jsonObject.title}Mesh";

            ctx.AddObjectToAsset("kenShapeMesh", newMesh);
            ctx.AddObjectToAsset("kenShapeModel", model);
            ctx.SetMainObject(model);
        }

        private Mesh CreateRawMesh(KenShapeModel model)
        {
            var path = "Packages/com.qorthos.bracedframework/Editor/KenShapeImporter/KenShapeMeshDefs.asset";
            KenShapeMeshDef meshDefs = AssetDatabase.LoadAssetAtPath<KenShapeMeshDef>(path);

            var newMesh = new Mesh();

            List<Vector3> vertices = new List<Vector3>();
            List<Color> colors = new List<Color>();
            List<Vector4> uvs = new List<Vector4>();
            List<Vector4> uv2s = new List<Vector4>();
            List<int> indices = new List<int>();

            int indexCount = 0;

            var modelRotation = Quaternion.Euler(new Vector3(RotateAxis ? -90 : 0f, 0, 0));

            foreach (var kenxel in model.Kenxels)
            {
                var refMesh = meshDefs.Meshes[kenxel.Shape];
                var kenxelRotation = Quaternion.Euler(new Vector3(0, 0, kenxel.Rotation));

                Vector3 kenxelPos = new Vector3(kenxel.Position.x, kenxel.Position.y, 0);
                kenxelPos = modelRotation * kenxelPos;

                for (int i = 0; i < refMesh.vertexCount; i++)
                {
                    var newVertex = kenxelRotation * refMesh.vertices[i];
                    newVertex += kenxel.Position;
                    newVertex.z *= kenxel.Depth;
                    newVertex = modelRotation * newVertex;

                    colors.Add(kenxel.Color);
                    vertices.Add(newVertex);

                    uvs.Add(new Vector4(1 + kenxel.HDRIntensity, refMesh.normals[i].z < 0 ? 1 : 0, 0, 0));
                    uv2s.Add(new Vector4(kenxelPos.x, kenxelPos.y, kenxelPos.z, 0));
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
            newMesh.SetUVs(0, uvs);

            //var backfaceCount = 0;
            //uvs.ForEach(x => { if (x.y == 1) backfaceCount++; });
            //Debug.Log($"BackfaceCount: {backfaceCount}");

            //newMesh.SetUVs(2, uv2s);
            newMesh.colors = colors.ToArray();
            newMesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
            newMesh.RecalculateBounds();
            newMesh.RecalculateNormals();
            newMesh.RecalculateTangents();

            return newMesh;
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