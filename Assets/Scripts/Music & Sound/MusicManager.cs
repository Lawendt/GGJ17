using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System;

public class MusicManager : Singleton<MusicManager>
{
    public AudioSource[] sources;
    public AudioClip[] tracks;
    int lastSource = 0;

    public enum MusicTypes
    {
        Reggae,
        Eletronic,
        Classic,
        Punk,
        None
    };


    void fadeOut(AudioSource source, float speed = 1.0f, Action action = null)
    {
        StartCoroutine(_fadeOut(source, speed, action));
    }

    void fadeIn(AudioSource source, Action action = null)
    {
        StartCoroutine(_fadeIn(source, action));
    }

    IEnumerator _fadeOut(AudioSource source, float speed, Action action = null)
    {
        while (source.volume > 0.01)
        {
            source.volume = Mathf.Lerp(source.volume, 0, 0.01f);
            yield return new WaitForEndOfFrame();
        }

        source.Stop();
        if (action != null)
            action();

    }

    IEnumerator _fadeIn(AudioSource source, Action action = null)
    {
        while (source.volume < 0.91)
        {
            source.volume = Mathf.Lerp(source.volume, 1, 0.01f);
            yield return new WaitForEndOfFrame();
        }

        source.volume = 1f;
        if (action != null)
            action();

    }

    // Use this for initialization
    void Start()
    {
        _dontDestroyOnLoad = true;
        _destroyExistentObject = true;
        lastSource = 0;
        sources[0].volume = 0;
        ChangeMusicType(EnemyType.None);
    }

    public void ChangeMusicType(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Classic:
                ChangeTrackTo(tracks[4]);
                break;
            case EnemyType.Punk:
                ChangeTrackTo(tracks[6]);
                break;
            case EnemyType.Reggae:
                ChangeTrackTo(tracks[3]);
                break;
            case EnemyType.Eletronic:
                ChangeTrackTo(tracks[5]);
                break;
            case EnemyType.None:
                ChangeTrackTo(tracks[2]);
                break;
            default:
                break;
        }
    }

    void fadeOutAllBut(int index)
    {
        Debug.Log("Fading out all but " + index);
        for (int i = 0; i < sources.Length; i++)
        {
            if(i != index)
            {
                fadeOut(sources[i],5);
            }
        }
    }

    void ChangeTrackTo(AudioClip clip)
    {
        StopAllCoroutines();
        fadeOutAllBut(-1);

        if (clip == null)
            return;

        //Stop last source
        sources[(++lastSource)%3].Stop();

        for (int i = 0; i < sources.Length; i++)
        {
            if(sources[i].isPlaying)
            {
                if (sources[i].clip == clip)
                {
                    if (sources[i].volume < 0.91)
                    {
                        fadeIn(sources[i]);
                    }
                    return;
                }
            }
            else
            {
                //Fadeout all other sources besides this one
                fadeOutAllBut(i);

                //Override new clip for playing
                sources[i].clip = clip;

                //Play new clip
                sources[i].Play();

                //Fade in source volume
                fadeIn(sources[i]);

                return;
            }   
        }

        

    }

}
