using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BracedFramework
{
    [CreateAssetMenu(fileName = "KenShapeMeshDef", menuName = "Defs/KenShapeMeshDef")]
    public class KenShapeMeshDef : ScriptableObject
    {
        public List<Mesh> Meshes;
    }
}

