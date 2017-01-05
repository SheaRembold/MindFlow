using UnityEngine;
using System.Collections;

public class BreathControl : MonoBehaviour
{
    public AudioSource audio;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!audio.isPlaying && audio.time == 0)
                audio.Play();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (audio.isPlaying)
                audio.Pause();
            else if (audio.time > 0)
                audio.UnPause();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            audio.Stop();
        }
    }
}
