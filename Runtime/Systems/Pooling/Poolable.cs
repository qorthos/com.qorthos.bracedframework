using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BracedFramework
{
    public class Poolable : MonoBehaviour
    {
        protected Action _killAction;
        internal bool SafePooledDestruction = false;

        public virtual void Init(Action killAction)
        {
            this._killAction = killAction;
        }

        public virtual void ForceRelease()
        {
            _killAction.Invoke();
        }

        protected virtual void OnDestroy()
        {
            // tell the pool that this is being destroyed unsafely
            if (!SafePooledDestruction)
                Debug.LogWarning("Poolable item destroyed?");

        }
    }
}
