using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //

    //the camera tries to center in the player.
    //behavior should handle:
    //normal.
    //it turns slight more to the side it is facing.

    PlayerHandler handler;

    public void GetPlayer(PlayerHandler player)
    {
        this.handler = player;
    }
    bool cameraLocked;
    public void LockCamera()
    {
        cameraLocked = true;
    }
    private void Start()
    {
        if (handler == null) handler = PlayerHandler.instance;
    }

    float x;
    float y;
    Vector3 velocity = Vector3.zero;
    float dampTime = 0.1f;

    float timeToFollowFalling = 0.4f;
    float currentTime;


    bool IsCameraTooFar()
    {
        float distance = Vector3.Distance(handler.transform.position, transform.position);
        return distance > 4;
    }

    public void PlaceCameraInPlayer()
    {
        Vector3 camPos = new Vector3(handler.transform.position.x + x, handler.transform.position.y + 0.5f + y, -20);
        transform.position = camPos;
    }


    private void FixedUpdate()
    {
        if (cameraLocked) return;
        if (handler.isGrounded())
        {
            Vector3 camPos = new Vector3(handler.transform.position.x + x, handler.transform.position.y + 0.5f + y, -20);
            transform.position = Vector3.SmoothDamp(transform.position, camPos, ref velocity, dampTime);
            currentTime = 0;
        }
        else if(IsCameraTooFar() && !handler.isFalling())
        {
            dampTime = 0.3f;
            Vector3 camPos = new Vector3(handler.transform.position.x + x, handler.transform.position.y + 0.2f + y, -20);
            transform.position = Vector3.SmoothDamp(transform.position, camPos, ref velocity, dampTime);
        }

        if (handler.isFalling())
        {
            if(currentTime > timeToFollowFalling)
            {                 
                Vector3 camPos = new Vector3(handler.transform.position.x, handler.transform.position.y + 0.2f, -20);
                transform.position = Vector3.SmoothDamp(transform.position, camPos, ref velocity, dampTime);
            }
            else
            {
                currentTime += Time.deltaTime;
            }

        }

    }

    public void ControlCameraHorizontal(int dir)
    {
        if(dir != 0)
        {
            x = 3.5f * dir;
            StopAllCoroutines();
            dampTime = 0.3f;
            StartCoroutine(DampProcess());
        }
    }

    IEnumerator DampProcess()
    {
        yield return new WaitForSeconds(0.2f);
        dampTime = 0.1f;
    }


    public void ControlCameraVertical(int dir)
    {
        if(dir == 1)
        {
            y = 3.8f;
        }
        if(dir == -1)
        {
            y = -3.8f;
        }
        if(dir == 0)
        {
            y = 0;
        }


    }



    //i shake the camera but i need to override the original method of the camera.
}
