using UnityEngine;
using System.Collections;

public class VRDeviceManager : MonoBehaviour
{
    void Awake()
    {
        if (UnityEngine.VR.VRDevice.isPresent)
        {
            GetComponent<OVRCameraRig>().enabled = true;
            GetComponent<OVRManager>().enabled = true;
            GetComponentInChildren<MouseLook>().enabled = false;
        }
        else
        {
            GetComponent<OVRCameraRig>().enabled = false;
            GetComponent<OVRManager>().enabled = false;
            GetComponentInChildren<MouseLook>().enabled = true;
        }
    }
}
