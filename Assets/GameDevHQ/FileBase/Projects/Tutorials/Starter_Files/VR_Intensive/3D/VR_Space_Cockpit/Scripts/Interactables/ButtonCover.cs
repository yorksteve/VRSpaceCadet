using Scripts.Interfaces;
using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Interactables
{
    public class ButtonCover : Buttons
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            _grabber = other.GetComponentInParent<OVRGrabber>().GetController();
            if (GestureManager.Instance.GestureState(_grabber).Equals(GestureManager.HandState.point))
            {
                base.OnTriggerEnter(other);
            }
        }

        protected override void OnLogic()
        {
            base.OnLogic();
        }

        protected override void OffLogic()
        {
            base.OffLogic();
        }
    }
}

