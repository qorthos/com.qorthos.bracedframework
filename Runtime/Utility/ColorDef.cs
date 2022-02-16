using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BracedFramework
{
    [CreateAssetMenu(fileName = "ColorDef", menuName = "Defs/ColorDef")]
    public class ColorDef : ScriptableObject
    {
        public Color Color;
    }
}