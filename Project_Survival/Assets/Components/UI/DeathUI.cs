using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    GameObject holder;

    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI title;

    [Separator("BUTTONS")]
    [SerializeField] Image loadLastSaveImage;
    [SerializeField] TextMeshProUGUI loadLastSaveText;
    [SerializeField] Image quitMenuImage;
    [SerializeField] TextMeshProUGUI quitMenuText;
    [SerializeField] Image quitDesktopImage;
    [SerializeField] TextMeshProUGUI quitDesktopText;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        
    }

    public void StartDeathUI()
    {
        //do the animation and the presentation of the ui.
        holder.SetActive(true);

        SetImageColor(background);
        SetImageColor(loadLastSaveImage);
        SetImageColor(quitMenuImage);
        SetImageColor(quitDesktopImage);

        SetTextColor(title);
        SetTextColor(loadLastSaveText);
        SetTextColor(quitMenuText);
        SetTextColor(quitDesktopText);

        StartCoroutine(FadeProcess());

    }

    IEnumerator FadeProcess()
    {
        //fade in the background.
        StartCoroutine(FadeInImageProcess(background));
        StartCoroutine(FadeInTextProcess(title));

        yield return new WaitForSeconds(2f);

        StartCoroutine(FadeInImageProcess(loadLastSaveImage));
        StartCoroutine(FadeInTextProcess(loadLastSaveText));

        StartCoroutine(FadeInImageProcess(quitMenuImage));
        StartCoroutine(FadeInTextProcess(quitMenuText));

        StartCoroutine(FadeInImageProcess(quitDesktopImage));
        StartCoroutine(FadeInTextProcess(quitDesktopText));

    }

    //bring those images.
    IEnumerator FadeInImageProcess(Image image)
    {
        
        while(image.color.a < 1)
        {
            image.color += new Color(0,0,0,0.01f);
            yield return new WaitForSeconds(0.001f);
        }

    }
    void SetImageColor(Image image)
    {
        image.gameObject.SetActive(true);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    IEnumerator FadeInTextProcess(TextMeshProUGUI text)
    {
        while (text.color.a < 1)
        {
            text.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.001f);
        }
    }
    void SetTextColor(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(true);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }


    public void DEBUGGCONTROLDEATHUI(bool choice)
    {
        holder.SetActive(choice);
    }


    
    public void LoadLastSave()
    {
        //start the loading.

        //we need to 
        Debug.Log("yo");
        holder.SetActive(false);
        PlayerHandler.instance.ClearBlock();
        UIHolder.instance.LoadTransition();

    }
    public void QuitToMenu()
    {
        //change teh scene.
        holder.SetActive(false);
        UIHolder.instance.SceneTransition(SceneIndex.MainMenu);
    }
    public void QuitToDesktop()
    {
        //close the game.
        holder.SetActive(false);
        Application.Quit();
    }

}
