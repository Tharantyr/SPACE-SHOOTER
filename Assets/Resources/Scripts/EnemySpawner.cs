using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    List<List<EnemySpawn>> listOfSpawnLists;

    private void OnEnable()
    {
        if (GameSystem.instance.gameState == GameSystem.GameState.Wave)
            SpawnWave(GameSystem.instance.Wave);
    }

    private void Update()
    {
        if (GameSystem.instance.gameState == GameSystem.GameState.GameOver)
        {
            GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[0]); // Play sound effect
            GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[9]); // Play sound effect

            foreach (GameObject o in GameSystem.instance.enemyList)
                o.GetComponent<Enemy>().WaveDeath();
            GameSystem.instance.enemyList = new List<GameObject>();
            gameObject.SetActive(false);
        }

        if (GameSystem.instance.Timer > 0)
        {
            if (!GameSystem.instance.freezeTimer)
                GameSystem.instance.Timer -= Time.deltaTime;
        }
        else
        {
            GameSystem.instance.player.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[0]); // Play sound effect
            GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[9]); // Play sound effect

            GameSystem.instance.Timer = GameSystem.instance.waveTime; // Reset time

            foreach (GameObject o in GameSystem.instance.enemyList) // Destroy remaining enemies
                o.GetComponent<Enemy>().WaveDeath();
            GameSystem.instance.enemyList = new List<GameObject>();

            GameSystem.instance.gameState = GameSystem.GameState.WaveOver; // Transition to WaveOver
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        listOfSpawnLists = new List<List<EnemySpawn>>(); // List containing all spawn lists
        List<EnemySpawn> spawnList0 = new List<EnemySpawn>(); // Spawn list for wave 1
        List<EnemySpawn> spawnList1 = new List<EnemySpawn>(); // Spawn list for wave 2
        List<EnemySpawn> spawnList2 = new List<EnemySpawn>(); // Spawn list for wave 3
        List<EnemySpawn> spawnList3 = new List<EnemySpawn>(); // Spawn list for wave 4
        List<EnemySpawn> spawnList4 = new List<EnemySpawn>(); // Spawn list for wave 5
        List<EnemySpawn> spawnList5 = new List<EnemySpawn>(); // Spawn list for wave 6
        List<EnemySpawn> spawnList6 = new List<EnemySpawn>(); // Spawn list for wave 7
        List<EnemySpawn> spawnList7 = new List<EnemySpawn>(); // Spawn list for wave 8
        List<EnemySpawn> spawnList8 = new List<EnemySpawn>(); // Spawn list for wave 9
        List<EnemySpawn> spawnList9 = new List<EnemySpawn>(); // Spawn list for wave 10

        // Create spawnList0
        #region
        spawnList0.Add(new EnemySpawn("NormalEnemy", 0));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 3));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 6));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 9));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 12));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 15));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 18));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 21));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 24));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 27));

        spawnList0.Add(new EnemySpawn("NormalEnemy", 30));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 30));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 33));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 33));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 36));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 36));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 39));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 39));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 42));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 42));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 45));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 45));

        spawnList0.Add(new EnemySpawn("NormalEnemy", 48));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 48));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 48));

        spawnList0.Add(new EnemySpawn("NormalEnemy", 51));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 51));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 51));

        spawnList0.Add(new EnemySpawn("NormalEnemy", 51));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 51));
        spawnList0.Add(new EnemySpawn("NormalEnemy", 51));
        #endregion

        // Create spawnList1
        #region
        spawnList1.Add(new EnemySpawn("Asteroid", 0));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 3));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 3));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 6));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 6));

        spawnList1.Add(new EnemySpawn("Asteroid", 6));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 9));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 9));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 12));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 12));

        spawnList1.Add(new EnemySpawn("Asteroid", 12));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 15));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 15));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 18));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 18));

        spawnList1.Add(new EnemySpawn("Asteroid", 18));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 21));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 21));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 24));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 24));

        spawnList1.Add(new EnemySpawn("Asteroid", 24));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 27));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 27));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 30));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 30));

        spawnList1.Add(new EnemySpawn("Asteroid", 30));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 33));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 33));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 33));

        spawnList1.Add(new EnemySpawn("Asteroid", 33));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 36));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 36));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 36));

        spawnList1.Add(new EnemySpawn("Asteroid", 36));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 39));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 39));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 39));

        spawnList1.Add(new EnemySpawn("Asteroid", 39));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 42));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 42));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 42));

        spawnList1.Add(new EnemySpawn("Asteroid", 42));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 45));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 45));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 45));

        spawnList1.Add(new EnemySpawn("Asteroid", 45));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 48));
        spawnList1.Add(new EnemySpawn("Asteroid", 51));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 51));

        spawnList1.Add(new EnemySpawn("Asteroid", 51));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 54));
        spawnList1.Add(new EnemySpawn("Asteroid", 54));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 54));

        spawnList1.Add(new EnemySpawn("Asteroid", 54));
        spawnList1.Add(new EnemySpawn("NormalEnemy", 57));
        #endregion

        // Create spawnList2
        #region
        spawnList2.Add(new EnemySpawn("Asteroid", 0));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 0));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 1));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 2));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 3));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 4));
        spawnList2.Add(new EnemySpawn("Asteroid", 6));

        spawnList2.Add(new EnemySpawn("Asteroid", 7));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 8));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 9));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 10));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 11));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 12));
        spawnList2.Add(new EnemySpawn("Asteroid", 14));

        spawnList2.Add(new EnemySpawn("Asteroid", 15));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 16));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 17));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 18));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 19));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 20));
        spawnList2.Add(new EnemySpawn("Asteroid", 22));

        spawnList2.Add(new EnemySpawn("Asteroid", 22.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 23f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 23.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 24f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 24.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 25f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 25.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 26f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 26.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 27f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 27.5f));
        spawnList2.Add(new EnemySpawn("Asteroid", 28f));

        spawnList2.Add(new EnemySpawn("Asteroid", 28.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 29f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 29.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 30f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 30.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 31f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 31.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 32f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 32.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 33f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 33.5f));
        spawnList2.Add(new EnemySpawn("Asteroid", 34f));

        spawnList2.Add(new EnemySpawn("Asteroid", 34.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 35.75f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 36f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 36.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 37));
        spawnList2.Add(new EnemySpawn("Asteroid", 37.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 38));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 38.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 39));

        spawnList2.Add(new EnemySpawn("Asteroid", 40));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 40.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 41f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 41.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 42));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 42.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 43));
        spawnList2.Add(new EnemySpawn("Asteroid", 43.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 43.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 44));

        spawnList2.Add(new EnemySpawn("Asteroid", 45));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 45.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 46));
        spawnList2.Add(new EnemySpawn("Asteroid", 46));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 46.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 47));
        spawnList2.Add(new EnemySpawn("Asteroid", 47));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 47.25f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 47.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 47.75f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 48f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 48.25f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 48.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 48.75f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 49f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 49.25f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 49.5f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 49.75f));
        spawnList2.Add(new EnemySpawn("ChaserEnemy", 50f));

        spawnList2.Add(new EnemySpawn("Asteroid", 50));
        spawnList2.Add(new EnemySpawn("Asteroid", 51));
        spawnList2.Add(new EnemySpawn("Asteroid", 52));
        spawnList2.Add(new EnemySpawn("Asteroid", 53));
        spawnList2.Add(new EnemySpawn("Asteroid", 54));
        spawnList2.Add(new EnemySpawn("Asteroid", 55));

        #endregion

        // Create spawnLift3
        spawnList3 = spawnList0.Concat(spawnList2).OrderBy(o => o.time).ToList(); // We can concatenate lists of EnemySpawns to create new waves!

        // Create spawnList4
        #region
        spawnList4.Add(new EnemySpawn("ChargerEnemy", 0));
        spawnList4.Add(new EnemySpawn("ChargerEnemy", 2));
        spawnList4.Add(new EnemySpawn("ChargerEnemy", 4));

        spawnList4.Add(new EnemySpawn("ChargerEnemy", 8));
        spawnList4.Add(new EnemySpawn("ChargerEnemy", 10));
        spawnList4.Add(new EnemySpawn("ChargerEnemy", 12));
        spawnList4.Add(new EnemySpawn("ChargerEnemy", 14));
        spawnList4.Add(new EnemySpawn("ChargerEnemy", 16));

        spawnList4.Add(new EnemySpawn("ChargerEnemy", 20));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 20));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 20.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 21));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 21.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 22));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 22.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 23));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 23.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 24));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 24.5f));

        spawnList4.Add(new EnemySpawn("ChargerEnemy", 25));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 25));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 25.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 26));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 26.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 27));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 27.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 28));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 28.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 29));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 29.5f));

        spawnList4.Add(new EnemySpawn("ChargerEnemy", 30));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 30));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 30.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 31));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 31.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 32));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 32.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 33));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 33.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 34));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 34.5f));

        spawnList4.Add(new EnemySpawn("ChargerEnemy", 35));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 35));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 35.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 36));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 36.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 37));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 37.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 38));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 38.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 39));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 39.5f));

        spawnList4.Add(new EnemySpawn("NormalEnemy", 40));
        spawnList4.Add(new EnemySpawn("ChargerEnemy", 40));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 40));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 40.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 41));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 41.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 42));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 42.5f));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 42.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 43));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 43.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 44));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 44.5f));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 45));

        spawnList4.Add(new EnemySpawn("NormalEnemy", 45));
        spawnList4.Add(new EnemySpawn("ChargerEnemy", 45));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 45));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 45.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 46));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 46.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 47));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 47.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 49));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 49.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 49.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 50f));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 50));

        spawnList4.Add(new EnemySpawn("NormalEnemy", 50));
        spawnList4.Add(new EnemySpawn("ChargerEnemy", 50));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 50));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 50.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 51));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 51.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 52));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 52.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 53));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 53.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 53.5f));
        spawnList4.Add(new EnemySpawn("ChaserEnemy", 54));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 54));

        spawnList4.Add(new EnemySpawn("NormalEnemy", 55));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 55.5f));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 56));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 56.5f));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 57));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 57.5f));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 58));
        spawnList4.Add(new EnemySpawn("NormalEnemy", 58.5f));
        #endregion

        // Create spawnList5
        spawnList5 = spawnList0.Concat(spawnList3).OrderBy(o => o.time).ToList();

        // Create spawnList6
        #region
        spawnList6.Add(new EnemySpawn("LaserEnemy", 0));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 2));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 4));

        spawnList6.Add(new EnemySpawn("LaserEnemy", 8));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 8));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 10));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 12));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 12));

        spawnList6.Add(new EnemySpawn("LaserEnemy", 14));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 14));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 16));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 18));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 18));

        spawnList6.Add(new EnemySpawn("Asteroid", 20));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 21));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 22));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 23));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 24));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 25));
        spawnList6.Add(new EnemySpawn("Asteroid", 26));

        spawnList6.Add(new EnemySpawn("Asteroid", 28));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 29));
        spawnList6.Add(new EnemySpawn("ChaserEnemy", 30));
        spawnList6.Add(new EnemySpawn("ChaserEnemy", 31));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 32));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 33));
        spawnList6.Add(new EnemySpawn("Asteroid", 34));

        spawnList6.Add(new EnemySpawn("Asteroid", 36));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 37));
        spawnList6.Add(new EnemySpawn("Asteroid", 38));
        spawnList6.Add(new EnemySpawn("ChaserEnemy", 39));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 40));
        spawnList6.Add(new EnemySpawn("NormalEnemy", 41));
        spawnList6.Add(new EnemySpawn("Asteroid", 42));

        spawnList6.Add(new EnemySpawn("LaserEnemy", 44));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 48));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 50));
        spawnList6.Add(new EnemySpawn("LaserEnemy", 54));
        #endregion

        // Create spawnList7
        spawnList7 = spawnList0.Concat(spawnList6).OrderBy(o => o.time).ToList();

        // Create spawnList8
        spawnList8 = spawnList1.Concat(spawnList4).OrderBy(o => o.time).ToList();

        // Create spawnList9
        spawnList9 = spawnList2.Concat(spawnList3).OrderBy(o => o.time).ToList();


        // Add all spawn lists to main list
        listOfSpawnLists.Add(spawnList0);
        listOfSpawnLists.Add(spawnList1);
        listOfSpawnLists.Add(spawnList2);
        listOfSpawnLists.Add(spawnList3);
        listOfSpawnLists.Add(spawnList4);
        listOfSpawnLists.Add(spawnList5);
        listOfSpawnLists.Add(spawnList6);
        listOfSpawnLists.Add(spawnList7);
        listOfSpawnLists.Add(spawnList8);
        listOfSpawnLists.Add(spawnList9);

        for (int i = 0; i < 100; i++) // Create random waves after wave 10
        {
            listOfSpawnLists.Add(listOfSpawnLists[Random.Range(0, 9)].Concat(listOfSpawnLists[Random.Range(0, 9)]).OrderBy(o => o.time).ToList());
        }
    }

    public void SpawnWave(int wave)
    {
        StartCoroutine(WaitThenSpawn(listOfSpawnLists[7]));
    }

    private IEnumerator WaitThenSpawn(List<EnemySpawn> le)
    {
        foreach (EnemySpawn e in le)
        {
            while (GameSystem.instance.waveTime - e.time < GameSystem.instance.Timer)
            {
                yield return null;
            }

            GameSystem.instance.enemyList.Add(ObjectPool.instance.GetPooledObject(e.tag));
        }
    }

    private class EnemySpawn
    {
        public string tag;
        public float time;

        public EnemySpawn(string tag, float time)
        {
            this.tag = tag;
            this.time = time;
        }
    }
}