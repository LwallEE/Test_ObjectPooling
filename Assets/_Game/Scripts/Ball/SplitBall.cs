using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBall : Ball
{
    private const float LOWER_POS_RANDOM = 0f;
    private const float UPPER_POS_RANDOM = 0.1f;
    protected override void OnTouchWall(Vector2 normalCollision, Vector2 incomingDirection)
    {
        SpawnBombBall(incomingDirection * -1f); //spawn first ball in the opposite direction of incoming direction
        SpawnBombBall(Vector2.Reflect(incomingDirection, normalCollision)); //spawn second ball in the reflect direction when contact surface
        Despawn();
    }

    void SpawnBombBall(Vector2 direction)
    {
      
       Vector2 offset = direction * Random.Range(LOWER_POS_RANDOM, UPPER_POS_RANDOM) ; //random offset for not overlap two ball, when the incoming direction collinear with normal vector of surface
       var ball =  ObjectPooling.Instance.Spawn<BombBall>(EPoolType.EBallBomb, Tf.position + (Vector3)offset);
       ball.OnInit(direction);
       ball.SetSpecialTouch();
    }
}
