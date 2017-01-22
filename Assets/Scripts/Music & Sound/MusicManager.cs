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

        //source.Stop();
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
            case EnemyType.None:
                ChangeSourceTo(0);
                break;
            case EnemyType.Classic:
                ChangeSourceTo(1);
                break;
            case EnemyType.Punk:
                ChangeSourceTo(2);
                break;
            case EnemyType.Eletronic:
                ChangeSourceTo(3);
                break;
            case EnemyType.Reggae:
                ChangeSourceTo(4);
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
                fadeOut(sources[i]);
            }
        }
    }

    void ChangeSourceTo(int clipid)
    {
        //Stop all coroutines for once!
        StopAllCoroutines();

        //Fade out all source files
        fadeOutAllBut(clipid);

        //Start source (if it has not started already)
        if (!sources[clipid].isPlaying)
            sources[clipid].Play();

        //Fade in source volume
        fadeIn(sources[clipid]);

    }

}
