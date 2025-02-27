using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsTxt;

    private float timeForUpdateFPS = 0.5f;
    private float currentTimeForUpdate;
    private void Update()
    {
        if (currentTimeForUpdate > 0)
        {
            currentTimeForUpdate -= Time.deltaTime;
        }
        else
        {
            UpdateFps();
            currentTimeForUpdate = timeForUpdateFPS;
        }
    }

    void UpdateFps()
    {
        fpsTxt.text = $"FPS: {Mathf.RoundToInt(1 / Time.deltaTime)}";

    }
}
