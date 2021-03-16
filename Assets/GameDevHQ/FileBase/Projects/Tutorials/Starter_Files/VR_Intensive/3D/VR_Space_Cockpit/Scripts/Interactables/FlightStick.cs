using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;


namespace Scripts.Interactables
{
    public class FlightStick : MonoBehaviour
    {
        [SerializeField] private GameObject _ship;
        [SerializeField] private GameObject _flightController;
        [SerializeField] private float _rotationStrength = 50f;
        [SerializeField] private float _torque = 5f;
        [SerializeField] private float _rotateSpeed = .5f;

        OVRInput.Controller _controller;
        OVRGrabber _grabber;

        private Vector3 _controllerPos;
        private Vector3 _grabOrigin;
        private Vector3 _movement;
        private Renderer _handRend;

        private bool _isGrabbed;
        private bool _grabbable;
        private bool _audioSent;



        private void OnEnable()
        {
            GameSceneManager.onEnableFreeFlight += ActivateScene;
        }

        private void Start()
        {
            _grabbable = true;// false;
            _audioSent = false;
        }

        private void ActivateScene()
        {
            _grabbable = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_grabbable == true)
            {
                _grabber = other.GetComponentInParent<OVRGrabber>();
            }

            if (_grabber != null)
            {
                _controller = _grabber.GetController();
                if (_audioSent == false)
                {
                    //AudioManager.Instance.ReceiveVO(18);
                    _audioSent = true;
                }
                StartCoroutine(GrabCheckRoutine());
            }
        }

        IEnumerator GrabCheckRoutine()
        {
            while (_grabbable == true && _isGrabbed == false)
            {
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _controller) > .5f)
                {
                    GrabBegin();
                    StartCoroutine(WhileGrabbedRoutine());
                }

                yield return null;
            }
        }

        IEnumerator WhileGrabbedRoutine()
        {
            while (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _controller) > .5f)
            {
                WhileGrabbed();

                yield return null;
            }

            GrabEnd();
        }

        private void GrabBegin()
        {
            _isGrabbed = true;
            _grabOrigin = _grabber.transform.position;
            _handRend = _grabber.GetComponentInChildren<Renderer>();
            _handRend.enabled = false;
        }

        private void WhileGrabbed()
        {
            _controllerPos = _grabber.transform.position;

            var movement = _grabOrigin - _controllerPos;
            _movement.x = -movement.z;
            _movement.y = 0;
            _movement.z = movement.x;

            transform.rotation = Quaternion.Euler(_movement * _rotationStrength);

            RotateShip(_movement);
        }

        private void RotateShip(Vector3 movement)
        {
            _flightController.transform.Rotate(movement * _torque);

            _ship.transform.rotation = Quaternion.Lerp(_ship.transform.rotation, _flightController.transform.rotation, _rotateSpeed);
        }

        private void GrabEnd()
        {
            transform.eulerAngles = Vector3.zero;
            _isGrabbed = false;
            _handRend.enabled = true;
        }

        private void OnDisable()
        {
            GameSceneManager.onEnableFreeFlight -= ActivateScene;
        }

    }
}

