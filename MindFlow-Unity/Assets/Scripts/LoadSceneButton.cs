using UnityEngine;
using System.Collections;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName;

    void Start()
    {
        GetComponent<VRStandardAssets.Utils.VRInteractiveItem>().OnClick += OnClick;
    }

    void OnClick()
    {
        Application.LoadLevel(sceneName);
    }
}