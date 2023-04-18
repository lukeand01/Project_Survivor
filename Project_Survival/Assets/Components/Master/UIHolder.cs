using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHolder : MonoBehaviour
{
    public static UIHolder instance;

    public PlayerGUI gui;
    public InventoryUI inventory;
    public MenuUI menu;
    public DeathUI death;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);


        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    [Separator("Transition Image")]
    [SerializeField] Image transitionImage;

    public void FadeTransition(Transform doorTarget)
    {
        StartCoroutine(FadeTransitionProcess(doorTarget));
    }

    IEnumerator FadeTransitionProcess(Transform doorTarget)
    {
        PlayerHandler.instance.AddBlock("Transition", PlayerHandler.BlockType.Complete);

        var tempColor = transitionImage.color;

        while (tempColor.a < 1)
        {
            tempColor.a += 0.01f;
            transitionImage.color = tempColor;
            yield return new WaitForSeconds(0.008f);
        }


        //turn off deathui.
        PlayerHandler.instance.transform.position = doorTarget.position;

        yield return new WaitForSeconds(0.5f);

        while (tempColor.a > 0)
        {
            tempColor.a -= 0.01f;
            transitionImage.color = tempColor;
            yield return new WaitForSeconds(0.008f);
        }

        PlayerHandler.instance.RemoveBlock("Transition");
    }

    public void LoadTransition()
    {
        StartCoroutine(LoadProcess());
    }
    IEnumerator LoadProcess()
    {

        PlayerHandler.instance.AddBlock("Transition", PlayerHandler.BlockType.Complete);

        var tempColor = transitionImage.color;

        while (tempColor.a < 1)
        {
            tempColor.a += 0.01f;
            transitionImage.color = tempColor;
            yield return new WaitForSeconds(0.008f);
        }

        SaveHandler.instance.Load("First");
        death.DEBUGGCONTROLDEATHUI(false);

        //things cant move before the thing

        yield return new WaitForSeconds(0.5f);

        while (tempColor.a > 0)
        {
            tempColor.a -= 0.01f;
            transitionImage.color = tempColor;
            yield return new WaitForSeconds(0.008f);
        }

        PlayerHandler.instance.RemoveBlock("Transition");
    }

    public void SceneTransition(SceneIndex scene)
    {
        StartCoroutine(SceneProcess(scene));
    }
    IEnumerator SceneProcess(SceneIndex scene)
    {
        PlayerHandler.instance.AddBlock("Transition", PlayerHandler.BlockType.Complete);

        var tempColor = transitionImage.color;

        while (tempColor.a < 1)
        {
            tempColor.a += 0.01f;
            transitionImage.color = tempColor;
            yield return new WaitForSeconds(0.008f);
        }

        //change to the scene.
        //load if there is a file.
        //unload the scene you are own first.
        




        //things cant move before the thing

        yield return new WaitForSeconds(0.5f);

        while (tempColor.a > 0)
        {
            tempColor.a -= 0.01f;
            transitionImage.color = tempColor;
            yield return new WaitForSeconds(0.008f);
        }

        PlayerHandler.instance.RemoveBlock("Transition");
    }

}
