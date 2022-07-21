using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraController : MonoBehaviour
{
    // current active camera so I can quickly access them
    public static VirtualCameraController currentCamera; 

    public CinemachineVirtualCamera thisCamera;
    public CinemachineVirtualCamera UIcamera;
    public CinemachineCameraOffset UICameraOffset;
    public bool Enabled
    {
        get { return thisCamera.enabled; }
        set
        {
            thisCamera.enabled = value;
            UIcamera.enabled = value;

            if(thisCamera.enabled) // whenever a camera is activated, set it as current camera
            {
                currentCamera = this;
            }
        }
    }    
}
