using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallLauncher : MonoBehaviour
{
    private Camera cam;
    private bool isMouseDown;
    [SerializeField] private float numberOfBallSPerSecond;
    [SerializeField] private float offsetY;
    private float currentCountDownToShoot;
    private void Awake()
    {
        cam = Camera.main;
        Application.targetFrameRate = 60;
    }


    private void Update()
    {
        CheckPlayerInput();
        CheckShooting();
    }
    Vector2 GetBottomMiddlePosOfScreen()
    {
        return cam.transform.position - (cam.orthographicSize-offsetY) * Vector3.up;
    }

    void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }
    }

    void CheckShooting()
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
        Vector2 direction = ((Vector2)cam.ScreenToWorldPoint(Input.mousePosition) - startPos);
        
        ObjectPooling.Instance.Spawn<Ball>(ballType, startPos).OnInit(direction);
    }

    EPoolType GetRandomBallType()
    {
        return Random.Range(0, 2) == 0 ? EPoolType.EBallBomb : EPoolType.EBallSplit;
    }
   
}
