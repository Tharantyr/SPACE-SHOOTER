using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuStartGame : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(StartGame());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Text>().color = new Color(0.76f, 0.76f, 0.76f, 1);
    }

    private IEnumerator StartGame() // Transition to game when clicking button
    {
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.UISounds[0]); // Play sound effect
        GameSystem.instance.player.GetComponent<Renderer>().sortingOrder = 1;
        CanvasGroup c = transform.parent.GetComponent<CanvasGroup>();
        CanvasGroup uiCanvas = GameObject.Find("UI").GetComponent<CanvasGroup>();
        GameSystem.instance.InitializePlayer(); // Reset values for player upon new game

        while (c.alpha > 0)
        {
            c.alpha -= 0.01f;
            uiCanvas.alpha += 0.01f;
            yield return null;
        }

        GameSystem.instance.gameState = GameSystem.GameState.WaveStart;
        transform.parent.gameObject.SetActive(false);
    }
}
