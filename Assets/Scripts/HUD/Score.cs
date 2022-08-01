using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    
    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
