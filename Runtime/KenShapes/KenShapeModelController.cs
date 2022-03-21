using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BracedFramework;

namespace BracedFramework
{
    public class KenShapeModelController : MonoBehaviour
    {
        public MeshRenderer MR;
        public MeshFilter MF;
        public KenShapeModel KenShapeModel;
        public Collider Collider;

        List<Kenxel> kenxels = new List<Kenxel>();

        public void Disintegrate()
        {
            Collider.enabled = false;
            MR.enabled = false;

            var manager = FindObjectOfType<KenxelManager>();
            kenxels.Clear();
            manager.AttachKenxels(KenShapeModel, MR.transform, ref kenxels);

            for (int i = 0; i < kenxels.Count; i++)
            {
                kenxels[i].Disintegrate();
            }
        }

        public void Explode(Vector3 explosionOrigin, float force)
        {
            Collider.enabled = false;
            MR.enabled = false;

            var manager = FindObjectOfType<KenxelManager>();
            kenxels.Clear();
            manager.AttachKenxels(KenShapeModel, MR.transform, ref kenxels);

            for (int i = 0; i < kenxels.Count; i++)
            {
                kenxels[i].Fade();

                var dir = kenxels[i].transform.position - explosionOrigin;
                dir.Normalize();
                kenxels[i].RB.AddForce(dir * force);
            }
        }

        void OnValidate()
        {
            MF.mesh = null;

            if (KenShapeModel != null)
                MF.mesh = KenShapeModel.mesh;
        }
    }
}