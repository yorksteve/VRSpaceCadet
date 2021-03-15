using Scripts.Interfaces;
using Scripts.Managers;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Interactables
{
    public class ChestButton : Buttons
    {
        private bool _sceneActive;
        private WaitForSeconds _flashYield = new WaitForSeconds(.5f);
        private Color[] _colorArray = { Color.red, Color.black };

        public static Action onChestOpened;


        private void OnEnable()
        {
            Reactor.onBrokenCoreRemoved += ActivateScene;
        }

        protected override void Start()
        {
            base.Start();
            _sceneActive = false;
        }

        private void ActivateScene()
        {
            Debug.Log("ChestButton::ActivateScene()");
            _sceneActive = true;
            _rend.materials[0].color = Color.red;
            StartCoroutine(FlashRoutine());
        }

        protected override void OnTriggerEnter(Collider other)
        {
            _grabber = other.GetComponentInParent<OVRGrabber>().GetController();
            if (GestureManager.Instance.GestureState(_grabber).Equals(GestureManager.HandState.point))
            {
                onChestOpened?.Invoke();
                base.OnTriggerEnter(other);
                _sceneActive = false;
            }
        }

        protected override void OnLogic()
        {
            base.OnLogic();
            _rend.materials[0].color = Color.green;
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
            Reactor.onBrokenCoreRemoved -= ActivateScene;
        }
    }
}

