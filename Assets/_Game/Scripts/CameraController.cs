using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private Camera cam;
    protected override void Awake()
    {
        base.Awake();
        cam = GetComponent<Camera>();
    }

    public float Height => cam.orthographicSize * 2;
    public float Width => Height * cam.aspect;
    
    public Vector2 BottomMiddlePos => (Vector2)cam.transform.position - Vector2.up * cam.orthographicSize;
    public Vector2 UpperMiddlePos => (Vector2)cam.transform.position + Vector2.up * cam.orthographicSize;
    public Vector2 LeftMiddlePos => (Vector2)cam.transform.position - Vector2.right * Width / 2;
    public Vector2 RightMiddlePos => (Vector2)cam.transform.position + Vector2.right * Width / 2;

    public Vector2 GetMousePositionWorld()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
