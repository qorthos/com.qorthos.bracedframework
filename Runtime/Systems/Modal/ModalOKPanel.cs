using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BracedFramework
{
    public class ModalOKPanel : MonoBehaviour, IModalPanel
    {
        public GameEventChannel GameEventChannel;
        public TMPro.TMP_Text PanelText;
        public TMPro.TMP_Text ButtonText;
        public Animator Animator;

        [SerializeField] UnityEvent _onFinished;
        bool _isShown = false;

        public bool IsShown => _isShown;
        public UnityEvent OnFinished => _onFinished;

        public void Button_Click()
        {
            if (_isShown == false)
                return;

            Hide();
        }

        public void Set(string panelMessage, string buttonMessage)
        {
            PanelText.text = panelMessage;
            ButtonText.text = buttonMessage;
        }

        public void Show()
        {
            Animator.SetTrigger("Show");
            //GameEventChannel.Broadcast(new ActivateBlurGEM());
        }

        public void Shown()
        {
            _isShown = true;
        }

        public void Hide()
        {
            Animator.SetTrigger("Hide");
            //GameEventChannel.Broadcast(new DisableBlurGEM());
        }

        public void Hidden()
        {
            OnFinished?.Invoke();
        }

    }
}