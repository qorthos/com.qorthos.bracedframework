using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BracedFramework
{
    public class TransitionSystem : MonoBehaviour
    {
        public GameEventChannel GameEventChannel;
        public Animator Animator;

        [ReadOnly] [SerializeField] TransitionGEM _cachedTransitionGEM;

        private void Awake()
        {
            GameEventChannel.RegisterListener<TransitionGEM>(OnTransition);
        }

        private void OnTransition(TransitionGEM arg0)
        {
            _cachedTransitionGEM = arg0;


            switch (arg0.TransitionOutEffect)
            {
                case TransitionEffectEnum.Fullscreen:
                    Animator.SetTrigger("FullscreenFadeOut");
                    break;

                case TransitionEffectEnum.Scissor:
                    Animator.SetTrigger("ScissorFadeOut");
                    break;

                case TransitionEffectEnum.Wheel:
                    Animator.SetTrigger("WheelFadeOut");
                    break;

                default:
                    Debug.LogError($"TransitionSystem does not implement: {arg0.TransitionOutEffect}");
                    Animator.SetTrigger("FullscreenFadeOut");
                    break;
            }

            Animator.SetBool("SnapFadeIn", false);
            Animator.SetBool("FullscreenFadeIn", false);
            Animator.SetBool("ScissorFadeIn", false);
            Animator.SetBool("WheelFadeIn", false);

            switch (arg0.TransitionInEffect)
            {
                case TransitionEffectEnum.Fullscreen:
                    Animator.SetBool("FullscreenFadeIn", true);
                    break;

                case TransitionEffectEnum.Scissor:
                    Animator.SetBool("ScissorFadeIn", true);
                    break;

                case TransitionEffectEnum.Wheel:
                    Animator.SetBool("WheelFadeIn", true);
                    break;

                case TransitionEffectEnum.None:
                    Animator.SetBool("SnapFadeIn", true);
                    break;

                default:
                    Debug.LogError($"TransitionSystem does not implement: {arg0.TransitionInEffect}");
                    Animator.SetBool("SnapFadeIn", true);
                    break;
            }
        }

        public void TransitionCloseComplete()
        {
            _cachedTransitionGEM.OnClose?.Invoke();

            if (_cachedTransitionGEM.NewSceneName != "")
            {
                SceneManager.LoadScene(_cachedTransitionGEM.NewSceneName);
            }
        }

        public void TransitionReOpenComplete()
        {
            _cachedTransitionGEM.OnReopen?.Invoke();
        }
    }

    public enum TransitionEffectEnum
    {
        None,
        Fullscreen,
        Scissor,
        Wheel,
    }


    public class TransitionGEM : EventArgs
    {
        public string NewSceneName = "";
        public TransitionEffectEnum TransitionOutEffect;
        public TransitionEffectEnum TransitionInEffect = TransitionEffectEnum.None;
        public Action OnClose;
        public Action OnReopen;
    }
}