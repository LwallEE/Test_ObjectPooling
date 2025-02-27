using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CacheCollider
{
    private static Dictionary<Collider2D, Ball> ballDict = new Dictionary<Collider2D, Ball>(); //cache ball for less use Getcomponent

    public static bool GetBallComponent(Collider2D collider,out Ball ball) //return true if the collider has Ball component, else return false
    {
        if (!ballDict.ContainsKey(collider))
        {
            var result = collider.GetComponent<Ball>();
            ballDict.Add(collider, result);
            ball = result;
        }
        else
        {
            ball = ballDict[collider];
        }

        return ball != null;
    }
}
