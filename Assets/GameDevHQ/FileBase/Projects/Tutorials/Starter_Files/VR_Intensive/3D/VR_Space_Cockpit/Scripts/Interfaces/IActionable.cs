using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Interfaces
{
    public interface IActionable
    {
        void Grabbed();
        void Released();
        void Hand(OVRGrabber hand);
    }
}

