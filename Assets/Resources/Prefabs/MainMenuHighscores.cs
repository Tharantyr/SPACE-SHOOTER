using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuHighscores : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(GoHighscores());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Text>().color = new Color(0.76f, 0.76f, 0.76f, 1);
    }

    private IEnumerator GoHighscores() // Transition to highscores when clicking button
    {
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.UISounds[0]); // Play sound effect
        CanvasGroup c = transform.parent.GetComponent<CanvasGroup>();
        GameObject highscores = ObjectPool.instance.GetPooledObject("Highscores");
        highscores.GetComponent<Highscores>().LoadScores();
        CanvasGroup highscoresCanvas = highscores.GetComponent<CanvasGroup>();
        CanvasGroup scoreListCanvas = highscores.transform.Find("ScoreList").GetComponent<CanvasGroup>();
        CanvasGroup nameInputCanvas = highscores.transform.Find("NameInput").GetComponent<CanvasGroup>();

        while (c.alpha > 0)
        {
            scoreListCanvas.alpha = 1; // Skip score entering and go straight to score list
            nameInputCanvas.alpha = 0;

            c.alpha -= Time.deltaTime;
            highscoresCanvas.alpha += Time.deltaTime;
            yield return null;
        }

        GameSystem.instance.gameState = GameSystem.GameState.Highscores;
        transform.parent.gameObject.SetActive(false);
    }
}
