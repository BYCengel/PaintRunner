using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{

    private int tutorialIndex;
    private Transform tutorial;

    [SerializeField] private Canvas currentCanvas;

    [SerializeField] private GameObject[] tutorialTexts = new GameObject[4];

    //main menu
    private void Start() {
        currentCanvas = FindObjectOfType<Canvas>();
        tutorial = currentCanvas.gameObject.transform.Find("Tutorial");
        Debug.Log(currentCanvas.name);
    }

    public void OnTutorialButtonClick(){
        tutorialIndex = 0;
        currentCanvas.gameObject.transform.Find("MainMenu").gameObject.SetActive(false);
        currentCanvas.gameObject.transform.Find("Tutorial").gameObject.SetActive(true);
        OnNextTutorialButtonClick();
    }

    public void OnNextTutorialButtonClick()
    {
        for(int i = 0; i < tutorialTexts.Length; i++){
            if(i == tutorialIndex){
                tutorialTexts[i].gameObject.SetActive(true);
            }else{
                tutorialTexts[i].gameObject.SetActive(false);
            }
        }
        tutorialIndex++;
        if(tutorialIndex >= tutorialTexts.Length){
            tutorialIndex = 0;
        }
    }

    public void OnPlayButtonClick(){
        SceneManager.LoadScene("Aybars infinite runner");
    }

    public void OnSettingsButtonClick(){
        currentCanvas.gameObject.transform.Find("MainMenu").gameObject.SetActive(false);
        currentCanvas.gameObject.transform.Find("Settings").gameObject.SetActive(true);
    }

    public void OnReturnToMainMenuButtonClick(Button button){
        button.gameObject.transform.parent.gameObject.SetActive(false);
        currentCanvas.gameObject.transform.Find("MainMenu").gameObject.SetActive(true);
    }

    public void OnQuitButtonClick(){
        Application.Quit();
    }

    //end scene
    public void OnPlayAgainButton(){
        SceneManager.LoadScene("Aybars infinite runner");
    }

    public void OnMainMenuButton(){
        SceneManager.LoadScene("StartScene");
    }

}
