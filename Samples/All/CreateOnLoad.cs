using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BracedFramework
{
    public class CreateOnLoad : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void LoadFirstSceneAtGameBegins()
        {
            GameObject main = Instantiate(Resources.Load("GlobalSystems")) as GameObject;
            if (main == null)
            {
                Debug.LogError("Cannot find 'GlobalSystems' object in Resources");
            }

            DontDestroyOnLoad(main);

            main.transform.SetAsFirstSibling();
        }
    }
}