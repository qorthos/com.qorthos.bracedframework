using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BracedFramework
{
    public class Openable : MonoBehaviour
    {        
        public Animator Animator;
        [ReadOnly] [SerializeField] bool _isOpen = false;

        public bool IsOpen { get => _isOpen; private set => _isOpen = value; }
        

        public void Close()
        {
            Animator.SetTrigger("Close");
            IsOpen = false;
        }

        public void Open()
        {
            Animator.SetTrigger("Open");
            IsOpen = true;
        }


    }

}