using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallLauncher : MonoBehaviour
{
    private bool isMouseDown;
    [SerializeField] private float numberOfBallSPerSecond;
    [SerializeField] private float offsetY;
    private float currentCountDownToShoot; //Count down time for the next shooting time
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }


    private void Update()
    {
        CheckPlayerInput();
        CheckShooting();
    }
    Vector2 GetBottomMiddlePosOfScreen()
    {
        return CameraController.Instance.BottomMiddlePos + Vector2.up* offsetY;
    }

    void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0)) //Check player Input, when touch the screen
        {
            isMouseDown = true;
            currentCountDownToShoot = 0f; //Immidiately shoot if player click
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }
    }

    void CheckShooting() //function for check time to next shooting
    {
        if (!isMouseDown) return;
        if (currentCountDownToShoot <= 0)
        {
            Shoot();
            currentCountDownToShoot = 1.0f/ numberOfBallSPerSecond;
        }
        else
        {
            currentCountDownToShoot -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        EPoolType ballType = GetRandomBallType();
        Vector2 startPos = GetBottomMiddlePosOfScreen();
        Vector2 direction = ((Vector2)CameraController.Instance.GetMousePositionWorld() - startPos);
        
        ObjectPooling.Instance.Spawn<Ball>(ballType, startPos).OnInit(direction);
    }

    EPoolType GetRandomBallType()
    {
        return Random.Range(0, 2) == 0 ? EPoolType.EBallBomb : EPoolType.EBallSplit;
    }
   
}
