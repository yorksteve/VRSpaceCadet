using Scripts.Interfaces;
using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Interactables
{
    public class CoffeeButton : Buttons
    {
        private bool _sceneActive;
        private WaitForSeconds _flashYield;
        private Color[] _colorArray = { Color.red, Color.black };

        public static Action onCoffeeButtonClicked;


        private void OnEnable()
        {
            GameSceneManager.onStartCoffee += StartScene;
        }

        protected override void Start()
        {
            base.Start();
            _sceneActive = false;
            _rend.materials[0].color = Color.red;
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
            _rend.materials[0].color = Color.green;
            onCoffeeButtonClicked();
        }

        protected override void OffLogic()
        {
            base.OffLogic();

            _rend.materials[0].color = Color.red;
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
            GameSceneManager.onStartCoffee -= StartScene;
        }
    }
}

