using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public GameSystem gameSystem;

    private void Awake()
    {
        if (GameSystem.instance == null)
            Instantiate(gameSystem);
    }
}
