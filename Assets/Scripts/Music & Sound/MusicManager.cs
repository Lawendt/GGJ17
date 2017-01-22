using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System;

public class MusicManager : Singleton<MusicManager>
{
    public AudioSource[] sources;
    public AudioClip[] tracks;
    int lastTrack;

    void fadeOut(AudioSource source, Action action = null)
    {
        StartCoroutine(_fadeOut(source, action));
    }

    void fadeIn(AudioSource source, Action action = null)
    {
        StartCoroutine(_fadeIn(source, action));
    }

    IEnumerator _fadeOut(AudioSource source, Action action = null)
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
    }


    void ChangeTrackTo(AudioClip clip)
    {
        StopAllCoroutines();
        
    }

}
