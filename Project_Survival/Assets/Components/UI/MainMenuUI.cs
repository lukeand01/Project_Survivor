using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] GameObject continueButton;
    [SerializeField] WarningWindow warning;

    private void Start()
    {
        
        continueButton.SetActive(SaveHandler.instance.FileExists("First"));        
    }

    public void Continue()
    {
        SaveHandler.instance.Load("First");
        if (GameHandler.instance != null) GameHandler.instance.ChangeScene(GameHandler.instance.currentScene);
    }

    public void PlayGame()
    {

        if (SaveHandler.instance.FileExists("First"))
        {
            warning.StartWarning("You will delete your own progress if you start again.");
            warning.EventWarningYes += ConfirmPlayGame;
            warning.EventWarningNo += warning.ClearEvents;
        }
        else
        {
            ConfirmPlayGame();
        }
        

    }

    void ConfirmPlayGame()
    {
        warning.ClearEvents();
        SaveHandler.instance.DeleteFile("First");
        if (GameHandler.instance != null) GameHandler.instance.ChangeScene((int)SceneIndex.MainScene);
    }

    public void Exit()
    {
        if (warning.isActive) return;

        Application.Quit();
    }



}
