using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUnit : MonoBehaviour
{
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI turn;
    [SerializeField] TextMeshProUGUI strenght;


    public void UpdateUI(StatusClass status)
    {
        turn.text = status.currentTurn.ToString();
        strenght.text = status.strenght.ToString();

    }

    public void UpdateOtherUI(float current, float total)
    {
        portrait.fillAmount = current / total;
        StopAllCoroutines();
        if(current <= 0)
        {
            StartCoroutine(ControlProcess());
        }
    }

    IEnumerator ControlProcess()
    {

        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

}
