using Scripts.Interfaces;
using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Interactables
{
    public class TakeOffButton : Buttons
    {
        private bool _sceneActive;
        private bool _takeOff;
        private WaitForSeconds _flashYield;
        private Color[] _colorArray = { Color.red, Color.black };


        public static Action onTakeOff;



        private void OnEnable()
        {
            GameSceneManager.onUnlockTakeOff += StartScene;
            GameSceneManager.onEnableFreeFlight += StartScene;
        }

        protected override void Start()
        {
            base.Start();
            _sceneActive = false;
            _takeOff = true;
            _flashYield = new WaitForSeconds(.5f);
        }

        private void StartScene()
        {
            _sceneActive = true;
            StartCoroutine(FlashRoutine());
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (_sceneActive == true)
            {
                _grabber = other.GetComponentInParent<OVRGrabber>().GetController();
                if (GestureManager.Instance.GestureState(_grabber).Equals(GestureManager.HandState.point))
                {
                    base.OnTriggerEnter(other);
                    _sceneActive = false;
                }
            }
        }

        protected override void OnLogic()
        {
            base.OnLogic();
            _rend.material.color = Color.green;
            if (_takeOff == true)
            {
                onTakeOff?.Invoke();
                _takeOff = false;
            }
            else
            {
                Debug.Log("GAME OVER");
            }
        }

        protected override void OffLogic()
        {
            base.OffLogic();
            _rend.material.color = Color.red;
        }

        IEnumerator FlashRoutine()
        {
            int i = 0;
            while (_sceneActive == true)
            {
                _rend.materials[0].color = _colorArray[i % 2];
                i++;
                yield return _flashYield;
            }
        }

        private void OnDisable()
        {
            GameSceneManager.onUnlockTakeOff -= StartScene;
            GameSceneManager.onEnableFreeFlight -= StartScene;
        }
    }
}


