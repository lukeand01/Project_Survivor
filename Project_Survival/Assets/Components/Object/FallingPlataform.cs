using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlataform : MonoBehaviour, ISaveable
{
    [SerializeField] float fallTimer;
    float currentFallTimer;
    float shakeModifier;
    Rigidbody2D rb;
    GameObject body;
    bool touched;
    bool done;

   

    Vector3 originalPos;

    //if it ever touches the player it starts its countdown.
    private void Awake()
    {
        originalPos = transform.position;
        body = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();
        shakeModifier = 1;
    }
    private void Update()
    {
        if (!touched) return;
        if (done) return;

        Debug.Log("other counting");

        if(currentFallTimer > fallTimer)
        {

            done = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            body.transform.position = originalPos;

        }
        else
        {
            currentFallTimer += Time.deltaTime;
            shakeModifier += Time.deltaTime * currentFallTimer * 0.001f;
        }
    }


    public void StartShake(float newShakeTimer)
    {
        
        fallTimer = newShakeTimer;
        touched = true;
        StartCoroutine(ShakeProcess());
    }

    IEnumerator ShakeProcess()
    {
        while(currentFallTimer < fallTimer)
        {
            float x = UnityEngine.Random.Range(-0.04f, 0.04f) * shakeModifier;
            float y = UnityEngine.Random.Range(-0.04f, 0.04f) * shakeModifier;
            body.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Player") return;
        if(done || touched) return;
        Debug.Log("it started just now");
        touched = true;
        StartCoroutine(ShakeProcess());

    }

    #region SAVE
    public object CaptureState()
    {

        return new SaveData
        {
            done = done

        };

    }

    public void RestoreState(object state)
    {
        var save = (SaveData)state;

        done = save.done;

        if (done)
        {
            StopAllCoroutines();
            touched = true;

            rb.bodyType = RigidbodyType2D.Dynamic;

        }
        else
        {
            StopAllCoroutines();
            touched = false;

            rb.bodyType = RigidbodyType2D.Static;
            body.transform.position = originalPos;
        }

    }
    [Serializable]
    struct SaveData 
    {
        public bool done;
    
    }


    #endregion
}
