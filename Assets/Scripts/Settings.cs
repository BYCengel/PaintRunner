using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    GameManager gameManager;

    public AudioMixer audioMixer;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        SetMusicVolume(gameManager.GetMusicVolume());
        SetSFXVolume(gameManager.GetSfxVolume());
    }

    public void SetMusicVolume(float volume){
        Debug.Log("music " + volume);
        audioMixer.SetFloat("musicVolume", volume);
        gameManager.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume){
        Debug.Log("sfx " + volume);
        audioMixer.SetFloat("SFXVolume", volume);
        gameManager.SetSfxVolume(volume);
    }
}
