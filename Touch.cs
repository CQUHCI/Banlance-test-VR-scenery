using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
 
public class Touch : MonoBehaviour
{
    public SteamVR_Action_Boolean action = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");
    public SteamVR_Behaviour_Pose pose;
    public SteamVR_Action_Vector2 remoter2 = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("remoter2");

    // Update is called once per frame
    void Update()
    {
        if (action.GetStateDown(pose.inputSource))
        {
            Debug.Log("ButtonDown");
        }
        if (action.GetStateUp(pose.inputSource))
        {
            Debug.Log("ButtonUp");
        }
        if (action.GetState(pose.inputSource))
        {
            Debug.Log("Button");
        }

        //只要手放在触控板上，即可触发
        if (remoter2.GetChanged(pose.inputSource))
        {
            //获取手指在触控板的坐标位置
            //Debug.Log("Axis=" + remoter2.GetAxis(pose.inputSource) + " || Axis Delta=" + remoter2.GetAxisDelta(pose.inputSource));
        }

        //if (action.GetLastStateDown(pose.inputSource)) {
        //    Debug.Log("ButtonDownLater");
        //}

    }
}
