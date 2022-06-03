using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BracedFramework
{
    public class BroadcastEvent<T> : UnityEvent<T>, IBroadcastEvent
    {
        public void Clear()
        {
            RemoveAllListeners();
        }
    }

    public interface IBroadcastEvent
    {
        void Clear();
    }
}