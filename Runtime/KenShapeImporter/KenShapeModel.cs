using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class KenShapeModel : ScriptableObject
{
    public Mesh mesh;
    public Vector3Int Size;
    public List<Kenxel> Kenxels = new List<Kenxel>();
    public float DepthMultiplier;
}

[System.Serializable]
public class Kenxel
{
    public Vector3 Position;
    public float Rotation;
    public int Depth;
    public Color Color;
    public int Shape;
}


