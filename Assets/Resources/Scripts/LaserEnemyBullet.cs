using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyBullet : Projectile
{
    protected override void Awake()
    {
        base.Awake();
        speed = 20;
        damage = 2;
    }
}
