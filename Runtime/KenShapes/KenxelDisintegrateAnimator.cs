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
    public class KenxelDisintegrateAnimator : KenxelAnimator
    {
        public enum DisentegrationStateEnum
        {
            Paused,
            Normal,
            Shrinking,
        }

        [Header("Self References")]
        public Kenxel Kenxel;

        [Header("Effects")]
        private DisentegrationStateEnum _state;

        public float LifeTime = 2f;
        float _lifeCounter;

        public float ShrinkTime = 0.5f;
        bool _isShrinking = false;

        public float ModelPauseTime = 0.5f;
        float _pauseTime;
        float _pauseCounter;

        public KenxelManager KenxelManager { get; set; }

        private void Awake()
        {
        }

        private void Update()
        {


            switch (_state)
            {
                case DisentegrationStateEnum.Paused:
                    State_Paused();
                    break;

                case DisentegrationStateEnum.Normal:
                    State_Normal();
                    break;

                case DisentegrationStateEnum.Shrinking:
                    State_Shrinking();
                    break;
            }
        }

        void State_Paused()
        {
            _pauseCounter += Time.deltaTime;
            if (_pauseCounter > _pauseTime)
            {
                _state = DisentegrationStateEnum.Normal;
                Kenxel.BoxCollider.enabled = true;
                Kenxel.RB.useGravity = true;
                Kenxel.RB.constraints = new RigidbodyConstraints();
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
                _state = DisentegrationStateEnum.Shrinking;
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
            _pauseCounter = 0;
            _state = DisentegrationStateEnum.Paused;

            float y = transform.localPosition.y * 16f + Kenxel.KenShapeModel.Size.y / 2f;

            _pauseTime = ModelPauseTime * y / Kenxel.KenShapeModel.Size.y;

            Kenxel.BoxCollider.enabled = false;
            Kenxel.RB.useGravity = false;
            Kenxel.RB.constraints = RigidbodyConstraints.FreezeAll;

        }

        private void OnDisable()
        {

        }

    }
}