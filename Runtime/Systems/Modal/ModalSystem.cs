using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BracedFramework
{
    public class ModalSystem : MonoBehaviour
    {
        public GameEventChannel GameEventChannel;
        public GameObject FullScreenRayCast;
        public GameObject Canvas;
        public GameObject ShownObject;

        public GameObject StandardModalOKPrefab;
        private Action _cachedAction;

        // Start is called before the first frame update
        void Start()
        {
            GameEventChannel.RegisterListener<ShowModalOKGEM>(OnShowModalOK);
            GameEventChannel.RegisterListener<ShowCustomModalGEM>(OnShowCustomModel);
        }


        private void OnShowModalOK(ShowModalOKGEM arg0)
        {
            FullScreenRayCast.SetActive(true);
            _cachedAction = arg0.OKAction;

            var prefab = arg0.Prefab;
            if (prefab == null)
                prefab = StandardModalOKPrefab;

            ShownObject = Instantiate(prefab, Canvas.transform);
            var newPanel = ShownObject.GetComponent<ModalOKPanel>();
            newPanel.Set(arg0.PanelMessage, arg0.ButtonMessage);
            newPanel.Show();
            newPanel.OnFinished.AddListener(OnPanelFinished);
        }


        private void OnShowCustomModel(ShowCustomModalGEM arg0)
        {
            FullScreenRayCast.SetActive(true);
            _cachedAction = arg0.OKAction;

            var prefab = arg0.Prefab;
            ShownObject = Instantiate(prefab, Canvas.transform);
            var newPanel = ShownObject.GetComponent<IModalPanel>();
            newPanel.Show();
            newPanel.OnFinished.AddListener(OnPanelFinished);
        }

        private void OnPanelFinished()
        {
            _cachedAction?.Invoke();
            FullScreenRayCast.SetActive(false);
            Destroy(ShownObject);
        }
    }

    public class ShowModalOKGEM : System.EventArgs
    {
        public GameObject Prefab; // null to use the standard one
        public string PanelMessage;
        public string ButtonMessage;
        public Action OKAction;
    }

    public class ShowCustomModalGEM : EventArgs
    {
        public GameObject Prefab;
        public Action OKAction;
    }
}