using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;


namespace Scripts.Managers
{
    public class PanelManager : MonoSingleton<PanelManager>
    {
        [SerializeField] private SpriteRenderer[] _screens;
        [SerializeField] private Material _screenOff;
        [SerializeField] private Material _screenMat;

        private bool _screensActive;

        public static Action onPowerStatus;


        public override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            _screensActive = true;
        }

        public void ScreenStatus()
        {
            onPowerStatus?.Invoke();

            if (_screensActive == true)
            {
                foreach (var screen in _screens)
                {
                    screen.material = _screenOff;
                }

                _screensActive = false;
            }
            else
            {
                foreach (var screen in _screens)
                {
                    screen.material = _screenMat;
                }

                _screensActive = true;
            }
            
        }
    }
}

