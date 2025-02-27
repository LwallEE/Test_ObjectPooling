using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : GameUnit
{
    protected Rigidbody2D _rigidbody;

    [SerializeField] protected float speed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void OnInit(Vector2 direction)
    {
        _rigidbody.velocity = direction.normalized * speed;
    }

    protected virtual void OnTouchWall(Vector2 normalCollision, Vector2 incomingDirection)
    {
        /*Vector2 reflectDirection = Vector2.Reflect(_rigidbody.velocity.normalized, normalCollision);
        Debug.Log(_rigidbody.velocity + " " + normalCollision + " " + reflectDirection);
        _rigidbody.velocity = reflectDirection * speed;*/
    }

    public void AddForce(Vector2 direction, float force)
    {
        _rigidbody.AddForce(direction*force, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Constant.WALL_TAG))
        {
            
            OnTouchWall(other.contacts[0].normal, other.relativeVelocity.normalized);
        }
    }
}
