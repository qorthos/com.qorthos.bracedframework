using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BracedFramework;
using DG.Tweening;
using System;

namespace BracedFramework
{
    public class Kenxel : MonoBehaviour
    {
        [Header("Self References")]
        public MeshRenderer MR;
        public BoxCollider BoxCollider;
        public Rigidbody RB;
        public List<KenxelAnimator> Animators;
        public KenxelDisintegrateAnimator DisintegrateAnimator;
        public KenxelFadeAnimator FadeAnimator;

        public KenxelManager KenxelManager { get; internal set; }
        public KenShapeModel KenShapeModel { get; private set; }
        public KenxelData KenxelData { get; private set; }

        MaterialPropertyBlock _propBlock;

        private void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
            BoxCollider.enabled = true;
            RB.useGravity = true;
            RB.velocity = Vector3.zero;
        }


        private void OnDisable()
        {
            for (int i = 0; i < Animators.Count; i++)
                Animators[i].enabled = false;
        }

        public void Disintegrate()
        {
            DisintegrateAnimator.enabled = true;
        }

        public void Fade()
        {
            FadeAnimator.enabled = true;
        }

        public void ForceRelease()
        {
            KenxelManager.ReturnKenxel(KenxelData.Shape, this);
        }

        public void SetKenxelData(KenShapeModel model, KenxelData kenxelData)
        {
            KenxelData = kenxelData;
            KenShapeModel = model;

            var pos = kenxelData.Position;
            pos.z = 0;
                        
            transform.localPosition = pos;
            transform.localScale = new Vector3(1, 1, kenxelData.Depth);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, kenxelData.Rotation));
            BoxCollider.center = new Vector3(0, 0, kenxelData.Position.z);
            //BoxCollider.size = new Vector3(1 / 16f, 1 / 16f, 1/16f);
            MR.transform.localPosition = new Vector3(0, 0, kenxelData.Position.z);

            _propBlock.SetColor("_Color", kenxelData.Color);
            _propBlock.SetColor("_BackfaceColor", model.BackfaceColor);
            _propBlock.SetFloat("_HDRIntensity", 1 + kenxelData.HDRIntensity);
            _propBlock.SetInt("_UseBackfaceColor", model.UseBackfaceColor ? 1 : 0);
            MR.SetPropertyBlock(_propBlock);

            RB.constraints = new RigidbodyConstraints();
        }
    }
}