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
        public List<KenxelData> Kenxels = new List<KenxelData>();
        public float DepthMultiplier;
        public bool UseBackfaceColor;
        public Color BackfaceColor;
    }
}