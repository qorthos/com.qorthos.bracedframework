using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BracedFramework
{
    public struct CameraSnapGEM
    {
        public string TargetName;
    }

    public struct GameQuitGEM
    {

    }

    public struct YarnArgs
    {
        public Action OnComplete;
        public string ErrorMessage;
    }

    //public class YarnScriptGEM : EventArgs
    //{
    //    public string NodeName;
    //    public string CallingObjectName;
    //    public string TargetObjectName;
    //}

}