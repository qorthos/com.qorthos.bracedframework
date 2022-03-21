using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BracedFramework
{
    public delegate void SpriteAnimatorEvent(SpriteAnimator sender);

    public class SpriteAnimator : MonoBehaviour
    {
        public MeshRenderer MR;
        public MeshRenderer MR2;
        public int DefaultTextureIndex;

        public float PPU = 32;
        public List<Texture2D> Textures;
        public List<SpriteSequence> Sequences;

        [SerializeField] [ReadOnly] private SpriteSequence _currentSequence;
        [SerializeField] [ReadOnly] private string _currentSequenceName;
        [SerializeField] [ReadOnly] private float _timer = 0;
        [SerializeField] [ReadOnly] private int _currentSpriteIndex = -1;

        Camera _cam;

        public event SpriteAnimatorEvent SequenceChanged;
        public event SpriteAnimatorEvent SequenceFinished;
        public event SpriteAnimatorEvent TextureChanged;

        public string CurrentSequenceName { get => _currentSequenceName; }

        void Start()
        {
            SetTexture(DefaultTextureIndex);
            _cam = Camera.main;
        }

        public void Update()
        {
            UpdateSequence();

            // the rotation needs to be set so that the sprite faces the camera.
            // mr2 is backface, it's flipped 180 degrees so shadows look correct.
            transform.rotation = Quaternion.AngleAxis(_cam.transform.rotation.eulerAngles.y, Vector3.up);
        }

        public void UpdateSequence()
        {
            if (_currentSequence == null)
                return;

            _timer += Time.deltaTime;

            if (_timer >= _currentSequence.Milliseconds / 1000f)
            {
                _timer -= _currentSequence.Milliseconds / 1000f;
                _currentSpriteIndex++;
                if (_currentSpriteIndex >= _currentSequence.Indices.Count)
                {
                    if (_currentSequence.Repeats)
                    {
                        _currentSpriteIndex = 0;
                    }
                    else
                    {
                        SequenceFinished?.Invoke(this);
                        return;
                    }
                }

                SetTexture(_currentSequence.Indices[_currentSpriteIndex]);
            }
        }

        void SetTexture(int newIndex)
        {
            float dppu = PPU * 2;
            float flipMulti = 1f;

            if (_currentSequence != null)
                flipMulti = _currentSequence.Flip ? -1f : +1f;

            var tex = Textures[newIndex];
            MR.material.SetTexture("_BaseMap", tex);

            MR.gameObject.transform.localScale = new Vector3(
                flipMulti * tex.width / PPU,
                tex.height / PPU,
                1);

            MR.gameObject.transform.localPosition = new Vector3(
                0,
                tex.height / dppu,
                0);

            MR2.material.SetTexture("_BaseMap", tex);

            MR2.gameObject.transform.localScale = new Vector3(
                -1 * flipMulti * tex.width / PPU,
                tex.height / PPU,
                1);

            MR2.gameObject.transform.localPosition = new Vector3(
                0,
                tex.height / dppu,
                0);

            TextureChanged?.Invoke(this);
        }

        public void PlaySequence(string name)
        {
            var sequence = Sequences.FirstOrDefault(x => x.Name == name);

            _currentSequence = sequence;
            _currentSequenceName = name;

            _timer = 0;
            _currentSpriteIndex = 0;

            if (sequence == null)
            {
                Debug.LogError($"Cannot play sequence {name}, no such sequence found in {gameObject.name}");

                return;
            }
            else
            {
                _currentSpriteIndex = _currentSequence.Indices[0];
                SetTexture(_currentSpriteIndex);
            }
        }
    }
}