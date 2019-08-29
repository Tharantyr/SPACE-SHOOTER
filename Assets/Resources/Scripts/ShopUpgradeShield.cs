using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUpgradeShield : ShopItem
{
    protected override void Awake()
    {
        cost = 5;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (GameSystem.instance.shieldCapacity == 100 || GameSystem.instance.Orbs < cost)
            GetComponent<CanvasGroup>().alpha = 0.2f;
        else
            GetComponent<CanvasGroup>().alpha = 1;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (GameSystem.instance.Orbs >= cost && GameSystem.instance.shieldCapacity < 100)
        {
            GameSystem.instance.shieldCapacity += 10;
            GameSystem.instance.Shield = GameSystem.instance.shieldCapacity;
            GameSystem.instance.Orbs -= cost;
            transform.Find("UpgradeShieldPic").Find("UpgradeShieldDesc").GetComponent<Text>().text = GameSystem.instance.shieldCapacity.ToString();
            base.OnTriggerEnter2D(col);

            if (GameSystem.instance.shieldCapacity == 100)
                StartCoroutine(FadeOut());
        }
    }
}
