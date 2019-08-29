using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    float deathTimer;

    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.white); // No fade-in effect for orbs
        deathTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (deathTimer < 3) // Destroy orb in 3 seconds
        {
            if (GameSystem.instance.gameState != GameSystem.GameState.PauseMenu)
                deathTimer += Time.deltaTime;

            if (deathTimer >= 2.5) // Make orb fade out after 2.5 seconds because it looks cool
            {
                Material mat = GetComponent<Renderer>().material;
                mat.SetColor("_Color", new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - Time.deltaTime * 2));
            }
        }
        else
            gameObject.SetActive(false);
    }
}
