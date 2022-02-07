using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BracedFramework
{
    [CreateAssetMenu(fileName = "ScriptChannel", menuName = "Channels/ScriptChannel")]
    public class ScriptChannel : ScriptableObject
    {
        public GameDataChannel GameDataChannel;
        public GameEventChannel GameEventChannel;

        [ReadOnly] public GameScripts GameScripts;


        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            if (GameScripts != null)
                Destroy(GameScripts.gameObject);
        }

        private void CreateSystem()
        {
            var scriptSystemGO = new GameObject("ScriptSystem", new Type[1] { typeof(GameScripts) });
            GameObject.DontDestroyOnLoad(scriptSystemGO);

            GameScripts = scriptSystemGO.GetComponent<GameScripts>();
            GameScripts.GameDataChannel = GameDataChannel;
            GameScripts.GameEventChannel = GameEventChannel;
        }

        public T PlayFunction<T>(string name, ScriptContext context)
        {
            if (GameScripts == null)
                CreateSystem();

            Debug.Log($"Attempting to play script: {name}");

            var type = typeof(GameScripts);
            var methodInfo = type.GetMethod(name);
            return (T)methodInfo.Invoke(GameScripts, new object[1] { context });
        }

        public void PlayScript(string name, ScriptContext context)
        {
            if (GameScripts == null)
                CreateSystem();

            Debug.Log($"Attempting to play script: {name}");

            GameScripts.StartCoroutine(name, context);
        }

        public void PlayScript(string name, object callingObject)
        {
            ScriptContext context = new ScriptContext()
            {
                CallingObject = callingObject
            };

            PlayScript(name, context);
        }
    }
}