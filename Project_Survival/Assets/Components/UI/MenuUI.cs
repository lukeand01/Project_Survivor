using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    GameObject holder;
    [SerializeField] WarningWindow warning;
    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;


    }

    private void Start()
    {
        if (SaveHandler.instance.FileExists("First"))
        {

        }
        else
        {

        }
    }

    public void ControlUI()
    {

        //whata happens if the time is slow?
        if (holder.activeInHierarchy)
        {
            holder.SetActive(false);
            PlayerHandler.instance.RemoveBlock("Pause");
        }
        else
        {
            holder.SetActive(true);
            PlayerHandler.instance.AddBlock("Pause", PlayerHandler.BlockType.SecondPartial);
        }
    }


    bool IsUsable()
    {

        if (warning != null)
        {
            if (warning.isActive) return false;
        }

        return true;
    }

    void ClearWarning()
    {
        warning.ClearEvents();
    }

    #region EXIT TO MENU
    public void ExitToMenu()
    {
        Debug.Log("clicked");
        if (!IsUsable()) return;
        Debug.Log("passed");

        warning.EventWarningYes += ConfirmExitToMenu;
        warning.EventWarningNo += ClearWarning;
        warning.StartWarning("any unsaved progress will be lost. are you sure?");
        
    }

    void ConfirmExitToMenu()
    {
        ClearWarning();
        Debug.Log("exit to menu");
        if (GameHandler.instance) GameHandler.instance.ChangeScene((int)SceneIndex.MainMenu);
    }
    
    
    #endregion

    #region EXIT TO DESKTOP
    public void ExitToDesktop()
    {
        if (!IsUsable()) return;
        warning.EventWarningYes += ConfirmExitToDesktop;
        warning.EventWarningNo += CancelExitToDesktop;
    }

    void ConfirmExitToDesktop()
    {
        warning.EventWarningYes -= ConfirmExitToDesktop;
        warning.EventWarningNo -= CancelExitToDesktop;

        Debug.Log("exit to desktop");
    }

   
    void CancelExitToDesktop()
    {
        warning.EventWarningYes -= ConfirmExitToDesktop;
        warning.EventWarningNo -= CancelExitToDesktop;
    }

    #endregion

    #region LOAD LAST SAVE
    public void LoadLastSave(bool choice = false)
    {
        if (!IsUsable()) return;

        if (!SaveHandler.instance.FileExists("First")) return;

        warning.StartWarning("Any Lost will be lost");
        warning.EventWarningYes += ConfirmLoadLastSave;
        //warning.EventWarningNo += CancelLoadLastSave;
    }
    void ConfirmLoadLastSave()
    {
        warning.ClearEvents();
        Debug.Log("load last save");
        PlayerHandler.instance.ClearBlock();
        if (GameHandler.instance) GameHandler.instance.ChangeScene(GameHandler.instance.currentScene);

    }
    void CancelLoadLastSave()
    {
        //warning.EventWarningYes -= ConfirmLoadLastSave;
        //warning.EventWarningNo -= CancelLoadLastSave;
    }
    #endregion



}
