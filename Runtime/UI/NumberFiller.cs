using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BracedFramework
{
    public class NumberFiller : MonoBehaviour
    {
        public Image FillImage;
        public TextMeshProUGUI Text;
        public ColorDef4 ColorDefs;

        public float TweenTime = 0.2f;
        public int MaxValue;
        public string TextPrefix;

        [SerializeField] private float _numberValue = 0;
        [ReadOnly][SerializeField] bool _hasStarted = false;

        public float NumberValue
        {
            get => _numberValue;
            set
            {
                if (_hasStarted)
                {
                    StopAllCoroutines();
                    StartCoroutine(TweenValue(_numberValue, value));
                }
                else
                {
                    _numberValue = value;
                    OnValidate();
                }
            }
        }

        void Start()
        {
            _hasStarted = true;
        }

        public IEnumerator TweenValue(float initial, float end)
        {
            var timer = TweenTime;

            while (timer > 0)
            {
                timer = Mathf.Clamp01(timer - Time.deltaTime);

                _numberValue = Mathf.Lerp(initial, end, 1 - timer / TweenTime);

                FillImage.fillAmount = Mathf.Clamp01(_numberValue / MaxValue);
                FillImage.color = Color.Lerp(ColorDefs.ColorA, ColorDefs.ColorB, Mathf.Clamp01(_numberValue / MaxValue));
                Text.color = Color.Lerp(ColorDefs.ColorC, ColorDefs.ColorD, Mathf.Clamp01(_numberValue / MaxValue));
                Text.text = $"{TextPrefix}{(int)_numberValue}";


                yield return new WaitForEndOfFrame();
            }


        }

        private void OnValidate()
        {
            FillImage.fillAmount = Mathf.Clamp01(_numberValue / MaxValue);
            FillImage.color = Color.Lerp(ColorDefs.ColorA, ColorDefs.ColorB, Mathf.Clamp01(_numberValue / MaxValue));
            Text.color = Color.Lerp(ColorDefs.ColorC, ColorDefs.ColorD, Mathf.Clamp01(_numberValue / MaxValue));
            Text.text = $"{TextPrefix}{(int)_numberValue}";
        }
    }
}