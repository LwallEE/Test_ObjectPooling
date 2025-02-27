using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustBorderScreen : MonoBehaviour //script for auto adjust the pos of border for different screen size
{
   [SerializeField] private Transform leftBorder;
   [SerializeField] private Transform rightBorder;
   [SerializeField] private Transform upBorder;
   [SerializeField] private Transform downBorder;

   [SerializeField] private float offset;
   private void Start()
   {
      Adjust();
   }

   void Adjust()
   {
      leftBorder.position = CameraController.Instance.LeftMiddlePos - Vector2.right*offset;
      rightBorder.position = CameraController.Instance.RightMiddlePos + Vector2.right*offset;
      upBorder.position = CameraController.Instance.UpperMiddlePos +Vector2.up*offset;
      downBorder.position = CameraController.Instance.BottomMiddlePos - Vector2.up*offset;
   }
}
