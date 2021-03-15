using Scripts.Interactables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;


namespace Scripts.Managers
{
    public class FadeManager : MonoSingleton<FadeManager>
    {
        [SerializeField] private Animator _anim;
        private WaitForSeconds _delay = new WaitForSeconds(.5f);
        private bool _startGame = true;

        public static Action onFading;


        private void OnEnable()
        {
            //ChairButton.onChairButtonClicked += Fade;
            GameSceneManager.onFade += Fade;
        }

        public override void Init()
        {
            base.Init();
        }

        public void Fade()
        {
            if (_startGame == true)
            {
                _anim.SetTrigger("Fade");
                _startGame = false;
            }
            else
            {
                StartCoroutine(FadeRoutine());
            }
        }

        IEnumerator FadeRoutine()
        {
            _anim.SetTrigger("Fade");
            yield return _delay;
            onFading?.Invoke();
        }

        private void OnDisable()
        {
            //ChairButton.onChairButtonClicked -= Fade;
            GameSceneManager.onFade -= Fade;
        }
    }
}

