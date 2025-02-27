using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallLauncher : MonoBehaviour //launcher ball, responsibility for check input of player and spawn ball
{
    private bool isMouseDown; //for control is player is holding in screen
    [SerializeField] private float numberOfBallSPerSecond; //speed for shoot
    [SerializeField] private float offsetY; //offset for the bottom pos of screen
    [SerializeField] private List<EPoolType> typeOfBallToSpawn; //all type of ball to spawn;
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
            currentCountDownToShoot = 0f; //Immediately shoot if player click
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }
    }

    void CheckShooting() //function for check time to next shooting
    {
        if (!isMouseDown) return; //if player isn't holding, return
        if (currentCountDownToShoot <= 0)
        {
            Shoot();
            currentCountDownToShoot = 1.0f/ numberOfBallSPerSecond; //function to calculate time wait for next shooting
        }
        else //reduce time until to the next shooting
        {
            currentCountDownToShoot -= Time.deltaTime;
        }
    }

    void Shoot() //function for shoot
    {
        EPoolType ballType = GetRandomBallType();
        Vector2 startPos = GetBottomMiddlePosOfScreen();
        Vector2 direction = ((Vector2)CameraController.Instance.GetMousePositionWorld() - startPos);//direction for shooting ball
        
        ObjectPooling.Instance.Spawn<Ball>(ballType, startPos).OnInit(direction);
    }

    EPoolType GetRandomBallType()
    {
        return typeOfBallToSpawn[Random.Range(0, typeOfBallToSpawn.Count)];
    }
   
}
