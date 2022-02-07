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

        [SerializeField] private float numberValue = 0;
        public float TweenTime = 0.2f;
        public int MaxValue;
        public string TextPrefix;

        bool hasStarted = false;

        public float NumberValue
        {
            get => numberValue;
            set
            {
                if (hasStarted)
                {
                    StopAllCoroutines();
                    StartCoroutine(TweenValue(numberValue, value));
                }
                else
                {
                    numberValue = value;
                    OnValidate();
                }
            }
        }

        void Start()
        {
            hasStarted = true;
        }

        public IEnumerator TweenValue(float initial, float end)
        {
            var timer = TweenTime;

            while (timer > 0)
            {
                timer = Mathf.Clamp01(timer - Time.deltaTime);

                numberValue = Mathf.Lerp(initial, end, 1 - timer / TweenTime);

                FillImage.fillAmount = Mathf.Clamp01(numberValue / MaxValue);
                FillImage.color = Color.Lerp(ColorDefs.ColorA, ColorDefs.ColorB, Mathf.Clamp01(numberValue / MaxValue));
                Text.color = Color.Lerp(ColorDefs.ColorC, ColorDefs.ColorD, Mathf.Clamp01(numberValue / MaxValue));
                Text.text = $"{TextPrefix}{(int)numberValue}";


                yield return new WaitForEndOfFrame();
            }


        }

        private void OnValidate()
        {
            FillImage.fillAmount = Mathf.Clamp01(numberValue / MaxValue);
            FillImage.color = Color.Lerp(ColorDefs.ColorA, ColorDefs.ColorB, Mathf.Clamp01(numberValue / MaxValue));
            Text.color = Color.Lerp(ColorDefs.ColorC, ColorDefs.ColorD, Mathf.Clamp01(numberValue / MaxValue));
            Text.text = $"{TextPrefix}{(int)numberValue}";
        }
    }
}