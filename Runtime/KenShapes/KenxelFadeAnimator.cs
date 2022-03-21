using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BracedFramework;
using DG.Tweening;

namespace BracedFramework
{
    public class KenxelFadeAnimator : KenxelAnimator
    {
        public enum FadeStateEnum
        {
            Normal,
            Shrinking,
        }

        [Header("Self References")]
        public Kenxel Kenxel;

        [Header("Effects")]
        private FadeStateEnum _state;

        public float LifeTime = 2f;
        float _lifeCounter;

        public float ShrinkTime = 0.5f;
        bool _isShrinking = false;

        public KenxelManager KenxelManager { get; set; }

        private void Awake()
        {
        }

        private void Update()
        {


            switch (_state)
            {
                case FadeStateEnum.Normal:
                    State_Normal();
                    break;

                case FadeStateEnum.Shrinking:
                    State_Shrinking();
                    break;
            }
        }

        void State_Normal()
        {
            _lifeCounter += Time.deltaTime;

            if (_lifeCounter > LifeTime)
            {
                Kenxel.ForceRelease();
            }

            if (_lifeCounter > LifeTime - ShrinkTime)
                _state = FadeStateEnum.Shrinking;
        }


        void State_Shrinking()
        {
            _lifeCounter += Time.deltaTime;

            if (_lifeCounter > LifeTime)
            {
                Kenxel.ForceRelease();
            }

            if (_isShrinking == false)
            {
                _isShrinking = true;
                //Kenxel.BoxCollider.enabled = false;
                //Kenxel.RB.useGravity = false;
                //Kenxel.RB.velocity = Vector3.zero;
                transform.DOScale(0.1f, ShrinkTime).SetEase(Ease.InBack);
            }
        }

        private void OnEnable()
        {
            _lifeCounter = 0;
            _isShrinking = false;
            _state = FadeStateEnum.Normal;
            Kenxel.BoxCollider.enabled = true;
            Kenxel.RB.useGravity = true;
        }

        private void OnDisable()
        {

        }

    }
}