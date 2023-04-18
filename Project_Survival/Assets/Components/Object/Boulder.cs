using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    //its not random its scripted.
    //a trigger can affect many boulders.

    [SerializeField] float fallSpeed;
    [SerializeField] float waitTimer;
    [SerializeField] Transform floorCheck;
    [SerializeField] GameObject body;
    float originalX = 0;

    bool isFalling;
    [SerializeField]DeathNet killingCollider;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalX = body.transform.position.x;
        killingCollider.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Fall();
        }
    }

    public void Fall()
    {
        //slowly and constantly falling.
        StartCoroutine(FallProcess(true));
    }

    public void ShakeAndFall()
    {
        StartCoroutine(ShakeProcess());
    }

    //but i want it to shake while it falls.
    //it needs to give the vibe of an old thing.

    IEnumerator ShakeProcess()
    {

        for (float i = 0; i < waitTimer; i += 0.05f)
        {
            Shake();
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(FallProcess(false));
    }
    IEnumerator FallProcess(bool shakesAsItFall)
    {
        while (!IsFloorBeneath())
        {
            //we descend
            transform.position -= new Vector3(0, 0.001f, 0) * fallSpeed;
            if (shakesAsItFall)
            {
                Shake();
            }

            //when the distance between the floor and its point is smaller than the player height.
            if(IsKillable() && !killingCollider.gameObject.activeInHierarchy)
            {
                Debug.Log("can kill now");
                killingCollider.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(0.01f);
        }     
    }

    void Shake()
    {
        float random = UnityEngine.Random.Range(-0.03f, 0.03f);
        body.transform.position = new Vector3(originalX + random, body.transform.position.y);
    }

    bool IsKillable()
    {
        return Physics2D.Raycast(floorCheck.position, Vector2.down, 0.4f, LayerMask.GetMask("Floor"));
    }

    bool IsFloorBeneath()
    {      
        return Physics2D.Raycast(floorCheck.position, Vector2.down, 0.05f, LayerMask.GetMask("Floor"));
    }



    
}
