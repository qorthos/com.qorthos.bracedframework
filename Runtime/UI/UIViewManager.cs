using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BracedFramework
{
    public class UIViewManager : MonoBehaviour
    {
        [ReadOnly] [SerializeField] UIViewController activeView;
        [ReadOnly] [SerializeField] UIViewController nextView;
        [ReadOnly] [SerializeField] bool _isBlocked = false;


        public void SetInitial(UIViewController initialView)
        {
            activeView = initialView;
        }

        public void ChangeView(UIViewController next)
        {
            if (_isBlocked)
                return;

            if (activeView == next)
                return;

            _isBlocked = true;
            nextView = next;
            StartCoroutine(ChangeViews());
        }

        private IEnumerator ChangeViews()
        {
            if (activeView != null)
            {
                activeView.Hide();
                activeView = null;
                //yield return new WaitForSeconds(1.0f);
            }

            if (nextView != null)
            {
                activeView = nextView;
                nextView = null;
                activeView.Show();

                //yield return new WaitForSeconds(1.0f);
            }

            _isBlocked = false;
            yield return null;
        }
    }
}