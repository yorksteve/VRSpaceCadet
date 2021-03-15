using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Interactables
{
    public class ExtraGrabbable : OVRGrabbable
    {
        private Renderer _handRend;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                this.gameObject.layer = 9;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                this.gameObject.layer = 8;
            }
        }

        public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
        {
            base.GrabBegin(hand, grabPoint);
            _handRend = grabbedBy.GetComponentInChildren<Renderer>();
            _handRend.enabled = false;
        }

        public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
        {
            base.GrabEnd(linearVelocity, angularVelocity);
            _handRend.enabled = true;
        }
    }
}

