using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour // Object pooling because it's less expensive than calling Instantiate() and Destroy() all the time
{
    public static ObjectPool instance;
    public List<GameObject> pooledObjects;

    private void Awake()
    {
        instance = this;

        pooledObjects = new List<GameObject>();

        // Initialize pool
        AddObjectsToPool("EnemySpawner", 1);
        AddObjectsToPool("MainMenu", 1);
        AddObjectsToPool("Shop", 1);
        AddObjectsToPool("Highscores", 1);
        AddObjectsToPool("Bullet", 5);
        AddObjectsToPool("Rocket", 5);
        AddObjectsToPool("NormalEnemyBullet", 10);
        AddObjectsToPool("LaserEnemyBullet", 10);
        AddObjectsToPool("Asteroid", 5);
        AddObjectsToPool("NormalEnemy", 5);
        AddObjectsToPool("ChaserEnemy", 5);
        AddObjectsToPool("ChargerEnemy", 5);
        AddObjectsToPool("ChargerEnemy", 5);
        AddObjectsToPool("LaserEnemy", 5);
        AddObjectsToPool("Explosion", 10);
        AddObjectsToPool("RocketExplosion", 5);
        AddObjectsToPool("AsteroidExplosion", 10);
        AddObjectsToPool("NormalEnemyExplosion", 10);
        AddObjectsToPool("Orb", 5);
    }

    public void AddObjectsToPool(string prefab, int n) // Instantiate objects at start of game and add them to a pool
    {
        for (int i = 0; i < n; i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/" + prefab));
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject(string tag) // Get a deactivated object from the pool and activate it, if there's none, instantiate a new one
    {
        for (int i = 0; i < pooledObjects.Count; i++)
            if (pooledObjects[i].tag == tag && !pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }

        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/" + tag));
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject(string tag, bool activated) // Control over whether to return activated/deactivated object
    {
        for (int i = 0; i < pooledObjects.Count; i++)
            if (pooledObjects[i].tag == tag && !pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(activated);
                return pooledObjects[i];
            }

        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/" + tag));
        obj.SetActive(activated);
        pooledObjects.Add(obj);
        return obj;
    }
}
