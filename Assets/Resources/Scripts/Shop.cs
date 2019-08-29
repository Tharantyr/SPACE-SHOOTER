using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    void OnEnable()
    {
        GameSystem.instance.gameState = GameSystem.GameState.Shop;
        GameSystem.instance.Health = 100;
        StartCoroutine(MoveToCenter(GameSystem.instance.player.transform.position)); // Fade in shop and move player to shop center
        transform.Find("OrbCount").GetComponent<Text>().text = "* " + GameSystem.instance.Orbs; // Update orb counter
    }

    private IEnumerator MoveToCenter(Vector2 startPosition) // Transition to shop and move player to center and lock him in place
    {
        GameSystem.instance.player.GetComponent<Renderer>().sortingOrder = 3;
        Rigidbody2D playerBody = GameSystem.instance.player.GetComponent<Rigidbody2D>();
        float startDistance = Vector2.Distance(playerBody.transform.position, new Vector2(0, -1.5f));
        float distance = startDistance;

        while (distance > 0.2f)
        {
            playerBody.position = Vector2.MoveTowards(playerBody.position, new Vector2(0, -1.5f), Time.deltaTime * 10);
            distance = Vector2.Distance(playerBody.transform.position, new Vector2(0, -1.5f));

            if (GetComponent<CanvasGroup>().alpha < 1)
            {
                float val = distance / startDistance;
                GetComponent<CanvasGroup>().alpha = 1 - val;
                GameSystem.instance.UI.transform.Find("TopUI").GetComponent<CanvasGroup>().alpha = val;
            }

            yield return null;
        }

        GetComponent<CanvasGroup>().alpha = 1;
        GameSystem.instance.UI.transform.Find("TopUI").GetComponent<CanvasGroup>().alpha = 0;
        playerBody.velocity = Vector2.zero;

        for (int i = 2; i < 8; i++)
        {
            transform.GetChild(i).GetComponent<ShopItem>().collider1.enabled = true;
            transform.GetChild(i).GetComponent<ShopItem>().collider2.enabled = true;
        }
    }

    public IEnumerator FadeShop() // Fade out shop upon exiting
    {
        GameSystem.instance.UI.transform.Find("WaveText").gameObject.SetActive(true);
        float shopAlpha = GetComponent<CanvasGroup>().alpha;

        while (shopAlpha > 0)
        {
            shopAlpha -= 0.003f;
            GetComponent<CanvasGroup>().alpha = shopAlpha;
            GameSystem.instance.UI.transform.Find("TopUI").GetComponent<CanvasGroup>().alpha += 0.003f;
            yield return null;
        }

        for (int i = 2; i < 8; i++)
        {
            transform.GetChild(i).GetComponent<Collider2D>().enabled = false; // Disable shop item bounding boxes so player doesn't collide with them when flying to center of shop next time
        }

        GameSystem.instance.gameState = GameSystem.GameState.WaveStart;
        GameSystem.instance.player.GetComponent<Renderer>().sortingOrder = 1;
        gameObject.SetActive(false);
    }
}
