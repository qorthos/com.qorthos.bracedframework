using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BracedFramework
{
    public class ModalCustomPanel : MonoBehaviour, IModalPanel
    {
        public GameEventChannel GameEventChannel;
        public Animator Animator;

        [SerializeField] UnityEvent _onFinished;
        bool _isShown = false;


        public UnityEvent OnFinished => _onFinished;
        public bool IsShown => _isShown;

        public void StartHide()
        {
            if (_isShown == false)
                return;

            Hide();
        }

        public void Show()
        {
            Animator.SetTrigger("Show");
            //GameEventChannel.Broadcast(new ActivateBlurGEM());
        }

        public virtual void Shown()
        {
            _isShown = true;
        }

        public void Hide()
        {
            Animator.SetTrigger("Hide");
            //GameEventChannel.Broadcast(new DisableBlurGEM());
        }

        public virtual void Hidden()
        {
            OnFinished?.Invoke();
        }

    }
}