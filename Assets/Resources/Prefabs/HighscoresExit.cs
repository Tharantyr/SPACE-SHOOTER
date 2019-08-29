using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HighscoresExit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(BackToMainMenu());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Text>().color = new Color(0.76f, 0.76f, 0.76f, 1);
    }

    private IEnumerator BackToMainMenu() // Transition back to main menu when pressing this button
    {
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.UISounds[0]); // Play sound effect
        CanvasGroup c = transform.parent.parent.GetComponent<CanvasGroup>();
        CanvasGroup mainMenuCanvas = ObjectPool.instance.GetPooledObject("MainMenu").GetComponent<CanvasGroup>();

        while (c.alpha > 0)
        {
            c.alpha -= Time.deltaTime;
            mainMenuCanvas.alpha += Time.deltaTime;
            yield return null;
        }

        GameSystem.instance.gameState = GameSystem.GameState.MainMenu;
        transform.parent.parent.gameObject.SetActive(false);
    }
}
