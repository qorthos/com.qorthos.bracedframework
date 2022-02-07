using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

namespace BracedFramework
{
    public class NumberSlider : MonoBehaviour
    {
        public Image FillImage;
        public TextMeshProUGUI Text;
        public ColorDef4 ColorDefs;
        public Slider Slider;

        [SerializeField] private float numberValue = 0;

        public float TweenTime = 0.2f;
        public int MaxValue;
        public string TextPrefix;

        private void Start()
        {
            Slider.onValueChanged.AddListener(OnSliderChanged);
        }

        private void OnSliderChanged(float arg0)
        {
            NumberValue = Mathf.Clamp(arg0, 0, MaxValue) * MaxValue;
        }

        public float NumberValue
        {
            get => numberValue;
            set
            {
                FillImage.fillAmount = Mathf.Clamp01(numberValue / MaxValue);
                FillImage.color = Color.Lerp(ColorDefs.ColorA, ColorDefs.ColorB, Mathf.Clamp01(numberValue / MaxValue));
                Text.color = Color.Lerp(ColorDefs.ColorC, ColorDefs.ColorD, Mathf.Clamp01(numberValue / MaxValue));
                Text.text = $"{TextPrefix}{(int)numberValue}";

                numberValue = value;
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