using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName;
    bool isFading;

    void Start()
    {
        GetComponent<VRStandardAssets.Utils.VRInteractiveItem>().OnClick += OnClick;
    }

    void OnClick()
    {
        if (isFading)
            return;

        isFading = true;
        VRCameraFade.Instance.OnFadeComplete += DoLoad;
        VRCameraFade.Instance.FadeOut(true);
    }

    void DoLoad()
    {
        VRCameraFade.Instance.OnFadeComplete -= DoLoad;
        Application.LoadLevel(sceneName);
    }
}