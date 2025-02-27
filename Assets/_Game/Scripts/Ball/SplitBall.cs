using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBall : Ball
{
    protected override void OnTouchWall(Vector2 normalCollision, Vector2 incomingDirection)
    {
        SpawnBombBall(incomingDirection * -1f);
        SpawnBombBall(Vector2.Reflect(incomingDirection, normalCollision));
        Despawn();
    }

    void SpawnBombBall(Vector2 direction)
    {
       var ball =  ObjectPooling.Instance.Spawn<BombBall>(EPoolType.EBallBomb, Tf.position);
       ball.OnInit(direction);
       ball.SetSpecialTouch();
    }
}
