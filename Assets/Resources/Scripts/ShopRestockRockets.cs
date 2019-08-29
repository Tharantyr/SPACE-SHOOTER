using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopRestockRockets : ShopItem
{
    protected override void Awake()
    {
        cost = 3;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (GameSystem.instance.Rockets == GameSystem.instance.rocketCapacity || GameSystem.instance.Orbs < cost)
            GetComponent<CanvasGroup>().alpha = 0.2f;
        else
            GetComponent<CanvasGroup>().alpha = 1;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (GameSystem.instance.Orbs >= cost && GameSystem.instance.Rockets < GameSystem.instance.rocketCapacity)
        {
            GameSystem.instance.Rockets = GameSystem.instance.rocketCapacity;
            GameSystem.instance.Orbs -= cost;
            base.OnTriggerEnter2D(col);

            if (GameSystem.instance.Rockets == GameSystem.instance.rocketCapacity)
                StartCoroutine(FadeOut());
        }
    }
}
