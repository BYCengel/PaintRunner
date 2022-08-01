using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    TestPlayer player;
    Transform playerTransform;
    Score scoreDisplay;
    private int highestScore = 0;
    private int currentScore = 0;

    private float musicVolume = -22f;
    private float sfxVolume = -22f;
    
    private bool isDead = false;

    [SerializeField] private GameObject startText;
    [SerializeField] private GameObject escapeText;

    private void Start() {
        if(SceneManager.GetActiveScene().name == "Aybars infinite runner"){
        player = FindObjectOfType<TestPlayer>();
        playerTransform = player.GetComponent<Transform>();
        scoreDisplay = FindObjectOfType<Score>();
        currentScore = 0;
        }
        if (SaveSystem.LoadScore() != null)
        {
            highestScore = SaveSystem.LoadScore().GetSavedHighScore();
        }
    }

    private void Update() {
        if(player != null && !isDead){
            SetCurrentScore((int) playerTransform.position.x); //calculates score
        }
    }

    public void CloseStartText(){
        startText.SetActive(false);
    }

    public void OpenEscapeText()
    {
        escapeText.SetActive(true);
        Invoke("CloseEscapeText", 2f);
    }

    private void CloseEscapeText()
    {
        escapeText.SetActive(false);
    }


    public void SetCurrentScore(int newScore){
        currentScore = newScore;
        scoreDisplay.SetScore(currentScore);
    }
    
    public void ManageHighestScore(){
        if(currentScore > highestScore){
            highestScore = currentScore;
        }
        SaveSystem.SaveScore(this);
        Invoke("LoadEndScene", 2f);
    }

    public int GetCurrentScore(){
        return currentScore;
    }
    
    public int GetHighestScore(){
        return highestScore;
    }

    public void LoadEndScene(){
        SceneManager.LoadScene("EndScene");
    }

    public void SetIsDead(bool value){
        isDead = value;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        SaveSystem.SaveScore(this);
    }
    
    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        SaveSystem.SaveScore(this);
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
