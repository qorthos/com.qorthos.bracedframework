using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BracedFramework
{
    [System.Serializable]
    public class KenShapeModel : ScriptableObject
    {
        public Mesh mesh;
        public Vector3Int Size;
        public List<Kenxel> Kenxels = new List<Kenxel>();
        public float DepthMultiplier;
    }
}