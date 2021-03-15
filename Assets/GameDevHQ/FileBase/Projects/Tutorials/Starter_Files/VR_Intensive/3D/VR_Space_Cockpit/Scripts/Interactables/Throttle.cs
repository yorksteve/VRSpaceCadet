using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;


namespace Scripts.Interactables
{
    public class Throttle : MonoBehaviour
    {
        [SerializeField] private GameObject _ship;
        [SerializeField] private float _rotationStrength = 50;
        [SerializeField] private float _speed = 1f;

        OVRInput.Controller _controller;
        OVRGrabber _grabber;

        private Vector3 _controllerPos;
        private Vector3 _movement;
        private Renderer _handRend;
        private Rigidbody _shipRB;
        private float _velocity;

        private bool _isGrabbed;
        private bool _grabbable;


        private void OnEnable()
        {
            GameSceneManager.onEnableFreeFlight += ActivateScene;
        }

        private void Start()
        {
            _grabbable = true;// false;
            _shipRB = _ship.GetComponent<Rigidbody>();
            if (_shipRB == null)
            {
                Debug.LogError("SHIP RIGIDBODY IS NULL");
            }
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
            _handRend = _grabber.GetComponentInChildren<Renderer>();
            _handRend.enabled = false;
        }

        private void WhileGrabbed()
        {
            _controllerPos = _grabber.transform.position;

            var movement = _controllerPos;
            _movement.x = movement.z;
            _movement.y = 0;
            _movement.z = 0;

            transform.rotation = Quaternion.Euler(_movement * _rotationStrength);
            Mathf.Clamp(-40f, 40f, transform.rotation.x);

            _velocity = transform.rotation.x + 40f;
            EnginePower(_velocity);
        }

        private void EnginePower(float movement)
        {
            _shipRB.velocity = new Vector3(0, 0, movement * _speed * Time.deltaTime);
            Vector3.Lerp(_shipRB.transform.position, _shipRB.velocity, .5f);
        }

        private void GrabEnd()
        {
            _isGrabbed = false;
            _handRend.enabled = true;
        }

        private void OnDisable()
        {
            GameSceneManager.onEnableFreeFlight -= ActivateScene;
        }
    }
}

