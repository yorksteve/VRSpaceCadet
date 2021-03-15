using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Scripts.Interactables
{
    public class Reactor : OVRGrabbable
    {
        [SerializeField] private bool _broken;

        private Renderer _rend;
        private bool _coreBroken;

        public static Action onBrokenCoreRemoved;
        public static Action onCoreReplaced;

        protected override void Start()
        {
            base.Start();
            _rend = GetComponent<Renderer>();
            if (_broken == true)
            {
                _coreBroken = true;
                StartCoroutine(FlickerRoutine());
            }
            else
            {
                _rend.materials[0].SetColor("_EmissionColor", Color.black);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Reactor") && _broken == false)
            {
                Debug.Log("CORE REPLACED");
                m_grabbedBy.ForceRelease(this);
                onCoreReplaced?.Invoke();
            }
        }

        public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
        {
            base.GrabBegin(hand, grabPoint);
        }

        public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
        {
            base.GrabEnd(linearVelocity, angularVelocity);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Reactor") && _broken == true)
            {
                onBrokenCoreRemoved?.Invoke();
                _coreBroken = false;
                Debug.Log("BROKEN CORE REMOVED");
                // Turn lights off
                _rend.materials[0].SetColor("_EmissionColor", Color.black);
            }
        }

        IEnumerator FlickerRoutine()
        {
            while (_coreBroken == true)
            {
                _rend.materials[0].SetColor("_EmissionColor", Color.red);
                yield return new WaitForSeconds(Random.Range(0f, .5f));
                _rend.materials[0].SetColor("_EmissionColor", Color.white);
            }
            yield return null;
        }
    }
}

