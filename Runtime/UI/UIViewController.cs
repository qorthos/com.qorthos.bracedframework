using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BracedFramework
{
    public class UIViewController : MonoBehaviour, IShowable
    {
        public bool StartHidden = true;

        public bool IsShown => throw new System.NotImplementedException();

        protected virtual void Start()
        {
            var rect = GetComponent<RectTransform>();

            if (StartHidden)
            {
                transform.position = transform.position + new Vector3(rect.rect.width, 0, 0);
            }
            else
            {
                FindObjectOfType<UIViewManager>().SetInitial(this);
            }
        }

        protected virtual void OnShown()
        {

        }

        protected virtual void OnHidden()
        {

        }

        public void Hide()
        {
            var rect = GetComponent<RectTransform>();

            var tween = transform.DOMove(transform.position + new Vector3(rect.rect.width, 0, 0), 0.33f)
                .SetEase(Ease.InOutQuad);
            tween.OnComplete(() => OnHidden());
        }

        public void Show()
        {
            var rect = GetComponent<RectTransform>();

            transform.position = transform.position - 2 * new Vector3(rect.rect.width, 0, 0);

            var tween = transform.DOMove(transform.position + new Vector3(rect.rect.width, 0, 0), 0.33f)
                .SetEase(Ease.InOutQuad);
            tween.OnComplete(() => OnShown());
        }
    }
}