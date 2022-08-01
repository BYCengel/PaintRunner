using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class End : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] TextMeshProUGUI highestScoreDisplay;

    void Start()
    {
        Time.timeScale = 1f;
        int currentScore = SaveSystem.LoadScore().GetSavedCurrentScore();
        int highestScore = SaveSystem.LoadScore().GetSavedHighScore();

        scoreDisplay.text = currentScore.ToString();
        highestScoreDisplay.text = highestScore.ToString();

    }

}
