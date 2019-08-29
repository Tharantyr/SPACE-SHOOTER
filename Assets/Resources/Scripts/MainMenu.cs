using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void OnEnable() // UI should be invisible in main menu
    {
        GameObject.Find("UI").GetComponent<CanvasGroup>().alpha = 0;
    }
}