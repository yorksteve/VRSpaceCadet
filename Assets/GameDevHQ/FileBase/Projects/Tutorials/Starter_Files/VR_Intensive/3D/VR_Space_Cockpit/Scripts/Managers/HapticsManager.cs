using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

public class HapticsManager : MonoSingleton<HapticsManager>
{

    public override void Init()
    {
        base.Init();
    }

    public void VibrateControllers()
    {
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
    }

}
