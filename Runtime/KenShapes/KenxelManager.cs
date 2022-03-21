using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BracedFramework;

namespace BracedFramework
{
    public class KenxelManager : MonoBehaviour
    {
        public List<Kenxel> KenxelPrefabs;
        ObjectPool<Kenxel>[] ksPools;

        private void Start()
        {
            ksPools = new ObjectPool<Kenxel>[KenxelPrefabs.Count];
            for (int i = 0; i < KenxelPrefabs.Count; i++)
            {
                int index = i;

                ksPools[i] = new ObjectPool<Kenxel>(
                    () =>
                    {
                        var kenxel = Instantiate(KenxelPrefabs[index], transform);
                        kenxel.KenxelManager = this;
                        return kenxel;
                    },
                    kenxel => { kenxel.gameObject.SetActive(true); kenxel.transform.parent = null; },
                    kenxel => { kenxel.gameObject.SetActive(false); kenxel.transform.parent = transform; },
                    kenxel => { Destroy(kenxel); },
                    false,
                    128,
                    1024);
            }
        }

        public void ReturnKenxel(int shape, Kenxel kenxel)
        {
            ksPools[shape].Release(kenxel);
        }

        public void AttachKenxels(KenShapeModel model, Transform destination, ref List<Kenxel> kenxels)
        {
            for (int j = 0; j < model.Kenxels.Count; j++)
            {
                var kenxelData = model.Kenxels[j];
                var newKenxel = ksPools[kenxelData.Shape].Get();
                newKenxel.transform.parent = destination;
                newKenxel.SetKenxelData(model, kenxelData);
                kenxels.Add(newKenxel);
            }
        }
    }
}