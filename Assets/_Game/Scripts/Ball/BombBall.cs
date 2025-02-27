using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using UnityEngine;

public class BombBall : Ball
{
   [SerializeField] private float timeForDestructed;
   [SerializeField] private float forceExplosion;
   [SerializeField] private LayerMask explosionImpactLayer;
   [SerializeField] private GameObject vfxPrefab;
   private int numberTouchWallToExplosion;
   private const int NORMAL_TOUCH_TO_DESTROY = 1;
   private const int SPECIAL_TOUCH_TO_DESTROY = 2;
   private const float EXPLOSION_RADIUS = 2f;
   private Collider2D[] ballInRangeResult = new Collider2D[20];
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
      numberTouchWallToExplosion -= 1;
      if (numberTouchWallToExplosion != 0)
      {
         return;
      }
      Invoke(nameof(Explosion), timeForDestructed);
   }

   void Explosion()
   {
      VfxPool.Instance.GetObj(vfxPrefab).transform.position = Tf.position;
      var size = Physics2D.OverlapCircleNonAlloc(Tf.position, EXPLOSION_RADIUS, ballInRangeResult, explosionImpactLayer);
      for (int i = 0; i < size; i++)
      {
         if (ballInRangeResult[i].TryGetComponent(out Ball ball))
         {
            ball.AddForce((ball.Tf.position -Tf.position).normalized, forceExplosion);
         }
      }
      Despawn();
   }
}
