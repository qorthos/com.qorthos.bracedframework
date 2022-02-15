using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BracedFramework;

namespace BracedFrameworkSample
{
    public class CreateOnLoad : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void LoadFirstSceneAtGameBegins()
        {
            var systems = new List<string>()
            {
                "BracedFrameworkSystems",
                "DialogueSystem",
            };

            foreach (string systemName in systems)
            {
                GameObject newSystem = Instantiate(Resources.Load(systemName)) as GameObject;
                if (newSystem == null)
                {
                    Debug.LogError($"Cannot find '{systemName}' object in Resources");
                }

                DontDestroyOnLoad(newSystem);

                newSystem.transform.SetAsFirstSibling();

            }

            GameDataChannel.Instance.GameCreated += GameDataChannel_GameCreated;

            CreateInitialData();
        }

        private static void GameDataChannel_GameCreated()
        {
            CreateInitialData();
        }

        private static void CreateInitialData()
        {
            Debug.Log("GameLoader Loading Default Data");
            // create game specific data in the GameDataChannel.Instance

        }
    }
}