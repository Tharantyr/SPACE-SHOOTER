using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuBack : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(Back());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Text>().color = new Color(0.76f, 0.76f, 0.76f, 1);
    }

    private IEnumerator Back() // Transition to main menu when pressing button
    {
        foreach (GameObject o in GameSystem.instance.enemyList) // Destroy all enemies in currently active enemy list
            o.GetComponent<Enemy>().WaveDeath();
        GameSystem.instance.enemyList = new List<GameObject>();

        GameSystem.instance.player.SetActive(false);
        GameSystem.instance.enemySpawner.SetActive(false);
        GameSystem.instance.shop.SetActive(false);

        CanvasGroup mainMenuCanvas = ObjectPool.instance.GetPooledObject("MainMenu").GetComponent<CanvasGroup>();

        while (mainMenuCanvas.alpha < 1)
        {
            mainMenuCanvas.alpha += 0.01f;
            yield return null;
        }

        GameSystem.instance.gameState = GameSystem.GameState.MainMenu;
        transform.parent.gameObject.SetActive(false);
    }
}
