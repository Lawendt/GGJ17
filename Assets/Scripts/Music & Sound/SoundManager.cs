using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager> {

    public AudioSource[] sources;
    public AudioClip[] effects;

    void Start()
    {
        _destroyExistentObject = true;
    }

    void play(AudioClip clip)
    {
        if (sources == null)
            return;
        if (sources.Length == 0)
            return;
        if (clip == null)
            return;

        AudioSource source = null;
        for (int i = 0; i < sources.Length; i++)
        {
            if (!sources[i].isPlaying)
                source = sources[i];
        }

        if (source == null)
            source = sources[0];

        source.pitch = Random.Range(0.8f, 1.2f);
        source.clip = clip;
        source.PlayOneShot(clip);
    }
}
