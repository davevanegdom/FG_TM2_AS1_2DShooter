using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class cs_MusicPlayer : MonoBehaviour
{

    public AudioSource Fullsong;
    public AudioSource Loopsong;

    public bool started;
    bool disable;
    public static cs_MusicPlayer instance;

    private void Start()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);    
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!disable && !Fullsong.isPlaying && !Loopsong.isPlaying)
        {
            if (!started)
            {
                Fullsong.Play();
                started = true;
            
            }
            else
            {
                Loopsong.Play();
                //Loopsong.loop = true;
                disable = true;
                //enabled = false;
            }

        }
    }

    IEnumerator WaitForSong()
    {
        if(!started)
        {
            Fullsong.Play();
            started = true;
            yield return new WaitForSeconds(Fullsong.clip.length);
            StartCoroutine(WaitForSong());
        }
        else
        {
            Loopsong.Play();
            yield return new WaitForSeconds(Loopsong.clip.length);
            StartCoroutine(WaitForSong());
        }
    }


}
