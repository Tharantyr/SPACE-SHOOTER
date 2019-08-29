using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class Highscores : MonoBehaviour
{
    InputField inputField;
    Coroutine c;

    private void OnEnable()
    {
        c = null;
        StartCoroutine(FadeScores());
    }

    private void Start()
    {
        inputField = transform.Find("NameInput").Find("InputField").GetComponent<InputField>();
    }

    public void GetName(string name) // Get name input
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            if (c == null)
            {
                GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.UISounds[0]); // Play sound effect
                SaveScore(GameSystem.instance.Score, GameSystem.instance.Wave, name.ToUpper());
                c = StartCoroutine(TransitionScores());
            }
    }

    public IEnumerator FadeScores() // Fade in highscores
    {
        foreach (GameObject e in GameSystem.instance.enemyList) // Kill all surviving enemies
            e.GetComponent<Enemy>().WaveDeath();
        GameSystem.instance.enemyList = new List<GameObject>();

        CanvasGroup c = GetComponent<CanvasGroup>();
        CanvasGroup ui = GameSystem.instance.UI.GetComponent<CanvasGroup>();
        CanvasGroup scoreListCanvas = transform.Find("ScoreList").GetComponent<CanvasGroup>();
        CanvasGroup nameInputCanvas = transform.Find("NameInput").GetComponent<CanvasGroup>();

        while (c.alpha < 1)
        {
            scoreListCanvas.alpha = 0;
            nameInputCanvas.alpha = 1; // Input score entry first
            c.alpha += Time.deltaTime;
            ui.alpha -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator TransitionScores() // Fade out name entry box and fade in score list
    {
        CanvasGroup scoreList = transform.Find("ScoreList").GetComponent<CanvasGroup>();
        CanvasGroup inputName = transform.Find("NameInput").GetComponent<CanvasGroup>();

        while (scoreList.alpha < 1)
        {
            scoreList.alpha += Time.deltaTime;
            inputName.alpha -= Time.deltaTime;
            yield return null;
        }
    }

    public void LoadScores() // Load scores when coming straight from main menu
    {
        if (File.Exists(Application.dataPath + "/highscores.json"))
        {
            string[] entryStrings = File.ReadAllLines(Application.dataPath + "/highscores.json");
            List<ScoreEntry> entryObjects = new List<ScoreEntry>();

            for (int i = 0; i < entryStrings.Length; i++)
                entryObjects.Add(JsonUtility.FromJson<ScoreEntry>(entryStrings[i]));

            entryObjects = entryObjects.OrderByDescending(o => o.score).ToList(); // Order by descending score

            for (int i = 0; i < 9; i++) // Display first 9 scores
            {
                Transform entryObject = transform.Find("ScoreList").Find("ScoreEntry" + (i + 1));
                ScoreEntry result = entryObjects[i];

                entryObject.Find("EntryRank").GetComponent<Text>().text = ConvertToRank(i + 1);
                entryObject.Find("EntryScore").GetComponent<Text>().text = result.score.ToString();
                entryObject.Find("EntryWaves").GetComponent<Text>().text = result.waves.ToString();
                entryObject.Find("EntryName").GetComponent<Text>().text = result.name;
            }
        }
    }

    private void SaveScore(int score, int waves, string name) // Save entry and display highscore list
    {
        if (File.Exists(Application.dataPath + "/highscores.json"))
        {
            string[] entryStrings = File.ReadAllLines(Application.dataPath + "/highscores.json");
            List<ScoreEntry> entryObjects = new List<ScoreEntry>();

            for (int i = 0; i < entryStrings.Length; i++)
                entryObjects.Add(JsonUtility.FromJson<ScoreEntry>(entryStrings[i]));

            entryObjects = entryObjects.OrderByDescending(o => o.score).ToList(); // Order by descending score

            ScoreEntry newEntry = new ScoreEntry { name = name, score = score, waves = waves }; // Add to JSON file
            string json = JsonUtility.ToJson(newEntry);
            File.AppendAllText(Application.dataPath + "/highscores.json", json + Environment.NewLine);

            bool inFirst9 = false;

            for (int i = 0; i < 9; i++) // Check if entry within first 9 and display first 9 scores
            {
                Transform entryObject = transform.Find("ScoreList").Find("ScoreEntry" + (i + 1));
                ScoreEntry result = entryObjects[i];

                if (newEntry != null)
                    if (newEntry.score > entryObjects[i].score)
                    {
                        inFirst9 = true;
                        result = newEntry;

                        entryObject.Find("EntryRank").GetComponent<Text>().color = new Color(1, 0.91f, 0, 1); // Make text color yellow to highligh entry
                        entryObject.Find("EntryScore").GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
                        entryObject.Find("EntryWaves").GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
                        entryObject.Find("EntryName").GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);

                        newEntry = null;
                    }

                entryObject.Find("EntryRank").GetComponent<Text>().text = ConvertToRank(i + 1);
                entryObject.Find("EntryScore").GetComponent<Text>().text = result.score.ToString();
                entryObject.Find("EntryWaves").GetComponent<Text>().text = result.waves.ToString();
                entryObject.Find("EntryName").GetComponent<Text>().text = result.name;
            }

            if (!inFirst9) // If not in first 9, place it at bottom
            {
                Transform entryObject;
                entryObject = transform.Find("ScoreList").Find("ScoreEntry9");
                entryObject.Find("EntryRank").GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
                entryObject.Find("EntryScore").GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
                entryObject.Find("EntryWaves").GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
                entryObject.Find("EntryName").GetComponent<Text>().color = new Color(1, 0.91f, 0, 1);
                entryObject.Find("EntryScore").GetComponent<Text>().text = newEntry.score.ToString();
                entryObject.Find("EntryWaves").GetComponent<Text>().text = newEntry.waves.ToString();
                entryObject.Find("EntryName").GetComponent<Text>().text = newEntry.name;

                for (int i = 9; i < entryObjects.Count(); i++)
                {
                    if (newEntry != null)
                        if (newEntry.score > entryObjects[i].score)
                        {
                            entryObject.Find("EntryRank").GetComponent<Text>().text = ConvertToRank(i);
                            return;
                        }
                }

                entryObject.Find("EntryRank").GetComponent<Text>().text = ConvertToRank(entryObjects.Count() + 1);
            }

        }
    }

    private string ConvertToRank(int rank) // Add st, nd, rd, or th to end of rank
    {
        int lastDigit = rank % 10;
        string result;

        if (lastDigit == 1)
            result = rank + "st";
        else if (lastDigit == 2)
            result = rank + "nd";
        else if (lastDigit == 3)
            result = rank + "rd";
        else
            result = rank + "th";

        // Special case for 11th, 12th, 13th
        lastDigit = rank % 100;

        if (lastDigit == 11 || lastDigit == 12 || lastDigit == 13)
            result = rank + "th";

        return result;
    }

    private class ScoreEntry // A class for saving and loading highscore entries
    {
        public int score, waves;
        public string name;
    }
}
