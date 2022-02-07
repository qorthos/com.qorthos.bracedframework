using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BracedFramework
{
    public static class TransformHelper
    {
        public static void Clear(Transform t)
        {
            var children = new List<GameObject>();
            for (int i = 0; i < t.childCount; i++)
            {
                children.Add(t.GetChild(i).gameObject);
            }
            foreach (var child in children)
            {
                GameObject.Destroy(child);
            }
        }
    }
}