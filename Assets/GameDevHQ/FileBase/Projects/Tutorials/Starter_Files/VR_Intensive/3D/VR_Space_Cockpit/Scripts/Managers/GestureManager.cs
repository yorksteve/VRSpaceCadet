using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class GestureManager : MonoSingleton<GestureManager>
    {
        public enum HandState
        {
            open,
            closed,
            point,
            //fingerGun,
            thumbsUp
        }

        //public enum Hand
        //{
        //    left,
        //    right
        //}

        public HandState handState;
        //public Hand hand;
        private OVRInput.Controller _controller;



        public override void Init()
        {
            base.Init();
        }

        //private void Start()
        //{
        //    if (hand.Equals(Hand.left))
        //    {
        //        _controller = OVRInput.Controller.LTouch;
        //    }
        //    else
        //    {
        //        _controller = OVRInput.Controller.RTouch;
        //    }
        //}

        public HandState GestureState(OVRInput.Controller controller)
        {
            _controller = controller;

            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _controller) > .5f)
            {
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, _controller) > .5f)
                {
                    if (OVRInput.Get(OVRInput.Touch.Two, _controller))
                    {
                        handState = HandState.closed;
                    }
                    else
                    {
                        handState = HandState.thumbsUp;
                    }
                }
            }

            if (OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger, _controller) == false)
            {
                handState = HandState.point;
            }

            return handState;
        }
    }
}

