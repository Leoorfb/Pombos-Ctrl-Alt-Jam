using System.Net.Sockets;
using System.Xml.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class Sound
{

    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range (.1f, 3)]
    public float minPitch = 1;
    [Range(.1f, 3)]
    public float maxPitch = 1;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    void Start(){
        Play("music");
    }

    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }
        
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + sound.name + " not found!");
            return;
        }

        sound.source.pitch = UnityEngine.Random.Range(sound.minPitch, sound.maxPitch);
        if (sound.loop)
        {
            sound.source.Play();
        }
        else
            sound.source.PlayOneShot(sound.clip);
    }
}
