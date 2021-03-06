using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BracedFramework
{
    public class Showable : MonoBehaviour, IShowable
    {
        [ReadOnly] [SerializeField] bool _isShown = false;
        public Animator Animator;
        public bool IsShown { get => _isShown; private set => _isShown = value; }


        public virtual void Hide()
        {
            Animator.SetTrigger("Hide");
            IsShown = false;
        }

        public virtual void Show()
        {
            Animator.SetTrigger("Show");
            IsShown = true;
        }


    }

}