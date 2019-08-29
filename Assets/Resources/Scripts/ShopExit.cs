using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopExit : ShopItem
{
    Coroutine fadeShop;

    // Update is called once per frame
    protected override void OnEnable()
    {
        itemPic = transform.GetChild(0);
    }

    protected override void Update()
    {
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        fadeShop = StartCoroutine(GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>().FadeShop());
    }
}
