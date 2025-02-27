using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBall : Ball
{
   [SerializeField] private float timeForDestructed; //time for destructed when touch wall
   [SerializeField] private float forceExplosion; //force apply to all ball in explosion's range
   [SerializeField] private LayerMask explosionImpactLayer; //layer mask for faster check ball in range
   [SerializeField] private GameObject vfxPrefab; //vfx when object is destructed
   private int numberTouchWallToExplosion; //current times of touch wall to explosion, explosion when the number go to 0
   private const int NORMAL_TOUCH_TO_DESTROY = 1; //number touch for normal case
   private const int SPECIAL_TOUCH_TO_DESTROY = 2; //number touch for special case when spawn after SplitBall is destroy
   private const float EXPLOSION_RADIUS = 2f;
   private Collider2D[] ballInRangeResult = new Collider2D[20]; //use non alloc for performance
   public override void OnInit(Vector2 direction)
   {
      base.OnInit(direction);
      numberTouchWallToExplosion = NORMAL_TOUCH_TO_DESTROY;
   }

   public void SetSpecialTouch()
   {
      numberTouchWallToExplosion = SPECIAL_TOUCH_TO_DESTROY;
   }
   protected override void OnTouchWall(Vector2 normalCollision,Vector2 incomingDirection)
   {
      base.OnTouchWall(normalCollision,incomingDirection);
      numberTouchWallToExplosion -= 1; //decrease each time touch wall
      if (numberTouchWallToExplosion != 0) // not handle if higher of lesser than 0, for some case invoke Explosion many times
      {
         return;
      }
      Invoke(nameof(Explosion), timeForDestructed);
   }

   void Explosion()
   {
      VfxPool.Instance.GetObj(vfxPrefab).transform.position = Tf.position; //spawn vfx in object position
      var size = Physics2D.OverlapCircleNonAlloc(Tf.position, EXPLOSION_RADIUS, ballInRangeResult, explosionImpactLayer); //find all ball in range
      for (int i = 0; i < size; i++)
      {
         if (CacheCollider.GetBallComponent(ballInRangeResult[i], out Ball ball) ) //use caching for less use GetComponent
         {
            ball.AddForce((ball.Tf.position -Tf.position).normalized, forceExplosion); //add force to ball in range
         }
      }
      Despawn(); //return to pool
   }
}
