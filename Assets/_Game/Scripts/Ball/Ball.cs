using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : GameUnit //base class of all ball
{
    protected Rigidbody2D _rigidbody;

    [SerializeField] protected float speed; //initial speed for moving

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void OnInit(Vector2 direction)
    {
        _rigidbody.velocity = direction.normalized * speed;
    }
    //virtual method for different behaviour of each ball
    protected virtual void OnTouchWall(Vector2 normalCollision, Vector2 incomingDirection) //call when ball touch wall, incoming Direction is the direction going to the wall
    {                                                                                       //normalCollision is normal direction of surface contact
        
    }

    public void AddForce(Vector2 direction, float force)
    {
        _rigidbody.AddForce(direction*force, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Constant.WALL_TAG))
        {
            
            OnTouchWall(other.contacts[0].normal, other.relativeVelocity.normalized*-1f);
        }
    }
}
