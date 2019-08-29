using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    protected override void Awake()
    {
        base.Awake();
        speed = 40;
        damage = 10;
    }
}
