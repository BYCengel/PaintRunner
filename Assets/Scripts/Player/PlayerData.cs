using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private int hiScore = 0;
    private int currentScore = 0;
    
    private float musicVolume;
    private float sfxVolume;
    

    public PlayerData(GameManager manager)
    {
        hiScore = manager.GetHighestScore();
        currentScore = manager.GetCurrentScore();
    }

    public int GetSavedHighScore()
    {
        return hiScore;
    }
    
    public int GetSavedCurrentScore()
    {
        return currentScore;
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }
    
    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetSfxVolume()
    {
        return sfxVolume;
    }
}
