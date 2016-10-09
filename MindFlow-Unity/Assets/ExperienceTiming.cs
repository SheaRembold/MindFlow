using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class ExperienceTiming : MonoBehaviour
{
    public GameObject[] step1On;
    public GameObject[] step1Off;
    public GameObject[] step2On;
    public Renderer person;
    public SpriteRenderer breath;
    public AudioSource audio;

    void Start()
    {
        for (int i = 0; i < step1On.Length; i++)
        {
            step1On[i].SetActive(false);
        }
        for (int i = 0; i < step2On.Length; i++)
        {
            step2On[i].SetActive(false);
        }

        StartCoroutine(DoTiming());
    }

    IEnumerator DoTiming()
    {
        yield return new WaitForSeconds(10f);
        
        VRCameraFade.Instance.FadeOut(true);

        while (VRCameraFade.Instance.IsFading)
            yield return new WaitForEndOfFrame();

        for (int i = 0; i < step1On.Length; i++)
        {
            step1On[i].SetActive(true);
        }
        for (int i = 0; i < step1Off.Length; i++)
        {
            step1Off[i].SetActive(false);
        }

        VRCameraFade.Instance.FadeIn(true);

        yield return new WaitForSeconds(4f);

        for (int i = 0; i < step2On.Length; i++)
        {
            step2On[i].SetActive(true);
        }

        /*float time = 0;
        Color personColor = person.material.color;
        float personAlpha = personColor.a;
        while (time < 1f)
        {
            personColor.a = Mathf.Lerp(0f, personAlpha, time);
            person.material.color = personColor;
            breath.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, time);
            yield return new WaitForEndOfFrame();
        }*/

        //audio.Play();
    }
}