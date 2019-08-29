using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Projectile : MonoBehaviour
{
    public int damage;

    protected Rigidbody2D body;
    protected float speed;
    Vector2 lastVelocity;
    bool frozen;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        frozen = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!GameSystem.instance.InView(gameObject))
            gameObject.SetActive(false);
    }

    protected virtual void FixedUpdate()
    {
        if (body.velocity.magnitude < speed) // Make sure velocity is always at least equal to speed
            body.velocity += body.velocity.normalized;

        if (GameSystem.instance.gameState == GameSystem.GameState.PauseMenu) // If game paused, freeze
        {
            if (!frozen)
            {
                lastVelocity = body.velocity;
                body.constraints = RigidbodyConstraints2D.FreezeAll;
                frozen = true;
            }
        }
        else
        {
            if (frozen)
            {
                body.constraints = RigidbodyConstraints2D.None;
                body.velocity = lastVelocity;
                frozen = false;
            }
        }
    }

    public virtual void SetDirection(Vector2 direction)
    {
        body.velocity = direction * speed;
    }
}