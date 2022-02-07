using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using System.Collections;

namespace BracedFramework
{
    public class GameScripts : MonoBehaviour
    {
        public GameDataChannel GameDataChannel;
        public GameEventChannel GameEventChannel;


        public IEnumerator TestFire(ScriptContext context)
        {
            Debug.Log("Test Fire!");

            yield return null;
        }

        public bool TestConditional(ScriptContext context)
        {
            return true;
        }

    }

    public class ScriptContext
    {
        public object CallingObject;
        public object[] ObjectData;
    }
}