using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBall : Ball
{
    private const float THRESHOLD_CHECK_SAME_DIRECTION = 0.2f;
    private const float LOWER_POS_RANDOM = 0.4f;
    private const float UPPER_POS_RANDOM = 0.6f;
    protected override void OnTouchWall(Vector2 normalCollision, Vector2 incomingDirection)
    {
        //Debug.Log(normalCollision + " " + incomingDirection*-1f);
        //bool isSameDirection = Vector2.Distance(incomingDirection * -1f, normalCollision) < THRESHOLD_CHECK_SAME_DIRECTION;
        SpawnBombBall(incomingDirection * -1f);
        SpawnBombBall(Vector2.Reflect(incomingDirection, normalCollision));
        Despawn();
    }

    void SpawnBombBall(Vector2 direction)
    {
      
       Vector2 offset = Vector2.one * Random.Range(LOWER_POS_RANDOM, UPPER_POS_RANDOM) ;
       var ball =  ObjectPooling.Instance.Spawn<BombBall>(EPoolType.EBallBomb, Tf.position + (Vector3)offset);
       ball.OnInit(direction);
       ball.SetSpecialTouch();
    }
}
