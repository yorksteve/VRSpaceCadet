using Scripts.Interfaces;
using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Interactables
{
    public class GenericButton : Buttons
    {
        protected override void Start()
        {
            base.Start();
            _rend.materials[0].color = Color.red;
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
            _rend.materials[0].color = Color.green;
            if (this.gameObject.CompareTag("Kraken"))
            {
                AudioManager.Instance.ReceiveVO(20);
            }
            if (this.gameObject.CompareTag("Weapons"))
            {
                AudioManager.Instance.ReceiveVO(17);
            }
        }

        protected override void OffLogic()
        {
            base.OffLogic();
            _rend.materials[0].color = Color.red;
        }
    }
}

