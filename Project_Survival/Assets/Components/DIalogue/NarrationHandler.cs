using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NarrationHandler : MonoBehaviour
{
    GameObject holder;
    [SerializeField] TextMeshProUGUI narrationText;
    [SerializeField] float writtingSpeed = 0.1f;
    [SerializeField] float transitionSpeed = 0.1f;
    bool isNarrating;
    Vector3 originalPos;
    //how do i know when to stop.

    private void Start()
    {
        originalPos = transform.position;
        holder = transform.GetChild(0).gameObject;
        Observer.instance.EventNarration += HandleNarration;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("this was called");
            StartNarration("this is a teste narration");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("this was called");
            StartNarration("second teste");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StopNarration();
        }
    }

    void HandleNarration(string narration)
    {
        if(narration == "")
        {
            StopNarration();
        }
        else
        {
            StartNarration(narration);
        }
    }

    void StartNarration(string narration)
    {
        StopAllCoroutines();
        StartCoroutine(OpenProcess(narration));
        StartCoroutine(NarrationProcess(narration));
    }

    void StopNarration()
    {
        StopAllCoroutines();
        StartCoroutine(CloseProcess());        
    }

    IEnumerator NarrationProcess(string narration)
    {
        isNarrating = true;
        narrationText.text = "";
        foreach (char letter in narration)
        {
            narrationText.text += letter;
            yield return new WaitForSeconds(writtingSpeed);
        }
        isNarrating = false;
    }

    IEnumerator OpenProcess(string narration)
    {
        while(transform.position.y > originalPos.y - 180)
        {
            transform.position -= new Vector3(0, 0.5f, 0);
            yield return new WaitForSeconds(transitionSpeed);
        }
        yield return new WaitForSeconds(0.2f);
    }
    IEnumerator CloseProcess()
    {
        while (transform.position.y < originalPos.y)
        {
            transform.position += new Vector3(0, 0.5f, 0);
            yield return new WaitForSeconds(transitionSpeed * 0.7f);
        }
    }
}
