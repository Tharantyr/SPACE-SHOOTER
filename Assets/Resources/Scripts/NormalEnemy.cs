using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    float lastBulletTime;

    protected override void Awake()
    {
        base.Awake();
        explosion = "NormalEnemyExplosion";
    }

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();
        hp = 30;
        lastBulletTime = -1; // Set to -1 otherwise we'll have to wait before allowed to fire
    }

    protected override void Spawn()
    {
        base.Spawn();
    }

    protected override void AI() // Program normal enemy AI
    {
        GameObject[] objectList = GameObject.FindObjectsOfType<GameObject>();
        float shortestDistance = 10000;

        for (int i = 0; i < objectList.Length; i++) // Find closest object to attack
        {
            if (objectList[i] != gameObject && (objectList[i].layer == 8 || objectList[i].layer == 9))
            {
                float distanceToObject = Vector2.Distance(transform.position, objectList[i].transform.position);
                if (distanceToObject < shortestDistance)
                {
                    shortestDistance = distanceToObject;
                    closestObject = objectList[i].transform;
                }
            }
        }

        if (closestObject != null) // Make sure there's still something left to kill
        {
            // Orientate towards closest object
            Vector3 direction = closestObject.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), 5 * Time.deltaTime);

            // Move closer to object
            if (Vector2.Distance(transform.position, closestObject.position) > 5)
                GetComponent<Rigidbody2D>().AddForce(direction.normalized * 5);

            // Shoot at closest object
            if (Time.time - lastBulletTime > 1f) // 1f is fire rate, which is 1/1 so 1 bullet per second
            {
                if (GameSystem.instance.gameState == GameSystem.GameState.PauseMenu) // Delay bullet fire to allow player to get back in the game after pause
                    lastBulletTime = Time.time + 1;
                else
                {
                    GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[Random.Range(5, 8)]); // Play sound effect
                    lastBulletTime = Time.time;
                    bullet = ObjectPool.instance.GetPooledObject("NormalEnemyBullet");

                    if (bullet != null)
                    {
                        bullet.transform.position = transform.position;
                        bullet.transform.rotation = transform.rotation;
                        Vector3 bulletDirection = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 90, Vector3.forward) * Vector3.right.normalized;
                        bullet.GetComponent<NormalEnemyBullet>().SetDirection(bulletDirection); // Set bullet direction
                    }
                }
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject != bullet) // Make sure enemy doesn't die from its own bullets
        {
            GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[Random.Range(1, 3)]); // Play sound effect
            base.OnTriggerEnter2D(col);
        }
    }

    protected override void Death()
    {
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[0]); // Play sound effect
        base.Death();

        if (lastHit == "Bullet" || lastHit == "RocketExplosion")
        {
            GameSystem.instance.Kills++;
            GameSystem.instance.Score += 50;
        }
    }

    public override void WaveDeath()
    {
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[0]); // Play sound effect
        base.WaveDeath();
    }
}
