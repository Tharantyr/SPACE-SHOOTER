using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        speed = 12;
        damage = 30;
    }

    public override void SetDirection(Vector2 direction)
    {
        body.velocity = direction; // Have rocket start at speed of 1 for nice speeding-up effect
    }
}
