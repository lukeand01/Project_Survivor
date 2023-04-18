using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour, ISaveable
{
    public static GameHandler instance;
    public int currentScene;
    
    [SerializeField] Image loadingbackground;
    [SerializeField] TextMeshProUGUI loadingText;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //scene transition.

    public void ChangeScene(int index)
    {
        StartCoroutine(StartProcess(index));
    }

    IEnumerator StartProcess(int index)
    {
        loadingbackground.gameObject.SetActive(true);
        var loadingBackgroundColor = loadingbackground.color;
        loadingBackgroundColor.a = 0;
        loadingbackground.color = loadingBackgroundColor;

        

        while(loadingBackgroundColor.a < 1)
        {
            loadingBackgroundColor.a += 0.01f;
            loadingbackground.color = loadingBackgroundColor;

            if(loadingBackgroundColor.a < 0.8f && !loadingText.gameObject.activeInHierarchy)
            {
                loadingText.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(0.01f);

        }

        SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.LoadSceneAsync(index);
        currentScene = index;

        if(index == 0)
        {
            StartCoroutine(ChangeMainMenuProcess());
        }
        else
        {
            StartCoroutine(ChangeSceneProcess());
        }
            
            
    }

    IEnumerator ChangeMainMenuProcess()
    {
        yield return null;
    }

    IEnumerator ChangeSceneProcess()
    {
        while(PlayerHandler.instance == null)
        {
            yield return null;
        }

        PlayerHandler.instance.AddBlock("Start", PlayerHandler.BlockType.Complete);

        while(SaveHandler.instance == null || LocalHandler.instance == null)
        {
            Debug.Log("waiting");
            yield return null;
        }

        if (SaveHandler.instance.FileExists("First"))
        {
            //then we load.
            SaveHandler.instance.Load("First");
        }
        else
        {
            LocalHandler.instance.StartLocal();
        }

        PlayerHandler.instance.cam.PlaceCameraInPlayer();

        yield return new WaitForSeconds(0.2f);

        //start fading the loading screen
        var loadingBackgroundColor = loadingbackground.color;


        while (loadingBackgroundColor.a > 0)
        {
            loadingBackgroundColor.a -= 0.01f;
            loadingbackground.color = loadingBackgroundColor;           
            yield return new WaitForSeconds(0.01f);

        }
        loadingText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);



        PlayerHandler.instance.RemoveBlock("Start");

    }



    #region SAVE
    public object CaptureState()
    {
        return new SaveData
        {
            mapIndex = currentScene
        };
    }

    public void RestoreState(object state)
    {
        var save = (SaveData)state;

        currentScene = save.mapIndex;
    }
    struct SaveData
    {
        public int mapIndex;
    }

    #endregion
}

public enum SceneIndex
{
    MainMenu = 0,
    MainScene = 1
}