using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BracedFramework
{
    [ExecuteInEditMode]
    public class UILookTowardsCamera : MonoBehaviour
    {
        GameObject _camTarget;

        private void Start()
        {
            _camTarget = Camera.main.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (_camTarget == null)
                return;

            transform.LookAt(transform.position + _camTarget.transform.forward);
        }
    }
}