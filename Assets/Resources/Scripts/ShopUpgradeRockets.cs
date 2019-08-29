using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUpgradeRockets : ShopItem
{
    protected override void Awake()
    {
        cost = 5;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (GameSystem.instance.rocketCapacity == 10 || GameSystem.instance.Orbs < cost)
            GetComponent<CanvasGroup>().alpha = 0.2f;
        else
            GetComponent<CanvasGroup>().alpha = 1;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (GameSystem.instance.Orbs >= cost && GameSystem.instance.rocketCapacity < 10)
        {
            GameSystem.instance.rocketCapacity++;
            GameSystem.instance.Orbs -= cost;
            transform.Find("UpgradeRocketsPic").Find("UpgradeRocketsDesc").GetComponent<Text>().text = GameSystem.instance.rocketCapacity.ToString();
            base.OnTriggerEnter2D(col);

            if (GameSystem.instance.rocketCapacity == 10)
                StartCoroutine(FadeOut());
        }
    }
}