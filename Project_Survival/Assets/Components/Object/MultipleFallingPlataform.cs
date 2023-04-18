using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MultipleFallingPlataform : MonoBehaviour
{
    //when you touch on this 

    //then we start shaking everyone.

    [SerializeField] FallingPlataform[] fallingPlataforms;

    [SerializeField] float fallTimer;
    float currentFallTimer;
    float shakeModifier;
    Rigidbody2D rb;
    GameObject body;
    bool touched;
    bool done;

    Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.position;
        body = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();
        shakeModifier = 1;
    }
    //we start the counter here.

    private void Update()
    {
        if (!touched) return;
        if (done) return;

        Debug.Log("couting");

        if (currentFallTimer > fallTimer)
        {
            Debug.Log("done");
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
    IEnumerator ShakeProcess()
    {
        while (currentFallTimer < fallTimer)
        {
            float x = Random.Range(-0.04f, 0.04f) * shakeModifier;
            float y = Random.Range(-0.04f, 0.04f) * shakeModifier;
            body.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Player") return;
        if (done || touched) return;
        touched = true;

        StartCoroutine(ShakeProcess());

        for (int i = 0; i < fallingPlataforms.Length; i++)
        {
            fallingPlataforms[i].StartShake(fallTimer);
        }

    }

}
