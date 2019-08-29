using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyBullet : Projectile
{
    protected override void Awake()
    {
        base.Awake();
        speed = 10;
        damage = 20;
    }
}
