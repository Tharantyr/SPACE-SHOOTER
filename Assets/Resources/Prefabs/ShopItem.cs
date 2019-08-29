using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Transform itemPic;
    protected Coroutine fade;
    protected int cost;
    bool sold;

    public void OnPointerEnter(PointerEventData eventData) // Make text yellow upon hovering mouse
    {
        GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
    }

    public void OnPointerExit(PointerEventData eventData) // Turn it white again
    {
        GetComponent<Text>().color = new Color(0.76f, 0.76f, 0.76f, 1);
    }

    protected virtual void Awake()
    {
        cost = 10;
    }

    protected virtual void OnEnable()
    {
        itemPic = transform.GetChild(0);
        transform.GetChild(1).GetComponent<Text>().text = "Cost: " + cost;
        sold = false;
        fade = null;
    }

    protected virtual void Update() // If player cannot afford item, make item invisible
    {
        if (GameSystem.instance.Orbs < cost)
            if (fade == null)
                fade = StartCoroutine(FadeOut());
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.UISounds[0]); // Play sound effect
        GameObject obj = ObjectPool.instance.GetPooledObject("NormalEnemyExplosion"); // Spawn explosion
        obj.transform.localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.GetChild(0).localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.GetChild(1).localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.GetChild(2).localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.position = GetComponent<Collider2D>().ClosestPoint(col.transform.position);
        obj.transform.rotation = transform.rotation;

        col.gameObject.SetActive(false);

        if (!sold)
            StartCoroutine(FlyToPlayer(itemPic.position));
    }

    protected virtual IEnumerator FlyToPlayer(Vector3 startPosition)
    {
        sold = true;
        cost += cost;
        transform.parent.Find("OrbCount").GetComponent<Text>().text = "* " + GameSystem.instance.Orbs; // Update orb counter

        if (name != "ExitShop")
            transform.GetChild(1).GetComponent<Text>().text = "Cost: " + cost; // Update cost

        GameObject player = GameSystem.instance.player;
        Transform picCopy = Instantiate(itemPic, itemPic.position, itemPic.rotation);
        picCopy.SetParent(itemPic.parent, false);
        Image image = picCopy.GetComponent<Image>();
        Text imageText = null;

        float startDistance = Vector2.Distance(picCopy.position, player.transform.position);
        float distance = startDistance;
        Vector3 direction = (player.transform.position - picCopy.position).normalized;

        if (picCopy.childCount > 0)
            imageText = picCopy.GetChild(0).GetComponent<Text>();

        while (distance > 0.2f)
        {
            picCopy.position += direction * Time.deltaTime * 500;
            distance = Vector2.Distance(picCopy.position, player.transform.position);
            Color c = image.color;
            c.a = distance / startDistance;
            image.color = c;

            if (imageText != null)
                imageText.color = c;

            yield return null;
        }

        sold = false;
        Destroy(picCopy.gameObject);
    }

    protected virtual IEnumerator FadeOut()
    {
        CanvasGroup c = GetComponent<CanvasGroup>();

        while (c.alpha > 0.2f)
        {
            c.alpha -= Time.deltaTime;
            yield return null;
        }
    }
}
