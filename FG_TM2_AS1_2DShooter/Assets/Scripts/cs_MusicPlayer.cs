using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class cs_MusicPlayer : MonoBehaviour
{

    public AudioSource Fullsong;
    public AudioSource Loopsong;

    public bool soundtrack = true;
    public bool loop = false;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Invoke("audioFinished", Fullsong.clip.length);
        }

    void audioFinished()
    {
        Debug.Log("Audio Finished");
    }
    public void Nonloop()
    {
        soundtrack = true;
        loop = false;
        Fullsong.Play();
    }

    public void Loop()
    {
        if (Fullsong.isPlaying)
            soundtrack = false;
        {
            Fullsong.Stop();
        }
        if  (!Loopsong.isPlaying && loop == false)
        {
            Loopsong.Play();
            loop = true;
        }
    }

    private void Update()
    {
        if (!Fullsong.isPlaying)
        {
            Loopsong.Play();
        }

    }


}
