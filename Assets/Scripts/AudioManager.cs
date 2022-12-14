using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    [SerializeField] private AudioMixerGroup SFXMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;


    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if(s.name.Contains("music")){
                s.source.outputAudioMixerGroup = musicMixerGroup;
            }else{
                s.source.outputAudioMixerGroup = SFXMixerGroup;
            }
        }
    }
    
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    private void Start()
    {
        Play("music1");
    }
}
