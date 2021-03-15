using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Interfaces;
using Scripts.Managers;

namespace Scripts.Interactables
{
    public class AutoPilotButton : Buttons
    {
        private bool _sceneActive;
        private WaitForSeconds _flashYield;
        private Color[] _colorArray = { Color.red, Color.black };



        private void OnEnable()
        {
            GameSceneManager.onEnableFreeFlight += StartScene;
        }

        protected override void Start()
        {
            base.Start();
            _sceneActive = false;
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
            Debug.Log("GAME OVER");
            // Engage autopilot
            // End game? or land ship?
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
            GameSceneManager.onEnableFreeFlight -= StartScene;
        }
    }

}
