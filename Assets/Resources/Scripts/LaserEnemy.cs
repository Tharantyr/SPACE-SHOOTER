using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : Enemy
{
    Coroutine c;
    bool firing;

    protected override void Awake()
    {
        base.Awake();
        explosion = "NormalEnemyExplosion";
    }

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();
        hp = 40;
        c = null;
        firing = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    protected override void Spawn()
    {
        if (GameSystem.instance.gameState != GameSystem.GameState.PauseMenu)
        {
            transform.position = ChooseLocation();
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2);

            while (colliders.Length > 0)
            {
                transform.position = ChooseLocation();
                colliders = Physics.OverlapSphere(transform.position, 1);
            }
        }
    }

    protected override Vector2 ChooseLocation()
    {
        int side = Random.Range(0, 3); // Choose random side of camera from which to enter scene
        float sidePosition = Random.Range(-4f, 4f) * 0.2f; // Spawn on random position on side
        float sideDestination = Random.Range(-4f, 4f) * 0.2f; // Move to random destination on other side
        float xbounds = render.bounds.extents.x;
        float ybounds = render.bounds.extents.y;

        Vector2 sideDest, result;

        if (side == 0) // Spawn on top side
        {
            result = new Vector2(Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect + xbounds, Camera.main.transform.position.y + sidePosition * Camera.main.orthographicSize);
            sideDest = new Vector2(Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect + xbounds, Camera.main.transform.position.y + sideDestination * Camera.main.orthographicSize);
        }
        else if (side == 1)// Spawn on right side
        {
            result = new Vector2(Camera.main.transform.position.x + sidePosition * Camera.main.orthographicSize * Camera.main.aspect, Camera.main.transform.position.y + Camera.main.orthographicSize - ybounds);
            sideDest = new Vector2(Camera.main.transform.position.x + sideDestination * Camera.main.orthographicSize * Camera.main.aspect, Camera.main.transform.position.y - Camera.main.orthographicSize - ybounds);
        }
        else if (side == 2) // Spawn on bottom side
        {
            result = new Vector2(Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect - xbounds, Camera.main.transform.position.y + sidePosition * Camera.main.orthographicSize);
            sideDest = new Vector2(Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect - xbounds, Camera.main.transform.position.y + sideDestination * Camera.main.orthographicSize);
        }
        else
        {
            result = new Vector2(Camera.main.transform.position.x + sidePosition * Camera.main.orthographicSize * Camera.main.aspect, Camera.main.transform.position.y - Camera.main.orthographicSize + ybounds);
            sideDest = new Vector2(Camera.main.transform.position.x + sideDestination * Camera.main.orthographicSize * Camera.main.aspect, Camera.main.transform.position.y + Camera.main.orthographicSize + ybounds);
        }

        return result;
    }

    protected override void AI() // Program normal enemy AI
    {
        if (GameSystem.instance.player != null) // Make sure there's still something left to kill
        {
            // Orientate towards player
            Vector3 direction = GameSystem.instance.player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (!firing)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), 5 * Time.deltaTime);

            if (c == null)
                c = StartCoroutine(FireLaser());
        }
    }

    private IEnumerator FireLaser()
    {
        yield return StartCoroutine(FadeToColor(Color.red, 1f));
        render.material.color = Color.white;
        Vector3 direction = GameSystem.instance.player.transform.position - transform.position;
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[10]); // Play sound effect

        while (render.material.color.a < 1)
        {
            Color color = render.material.color;
            color.a += Time.deltaTime;
            render.material.color = color;
            yield return null;
        }

        transform.GetChild(0).gameObject.SetActive(true);
        firing = true;
        float t = 0;

        while (t < 2)
        {
            if (GameSystem.instance.gameState != GameSystem.GameState.PauseMenu)
            {
                t += 0.1f;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.tag == "Player")
                    {
                        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.PlayerSounds[8]); // Play sound effect
                        hit.collider.GetComponent<Player>().DecreaseHealthShield(2);
                        StartCoroutine(hit.collider.GetComponent<Player>().Flash(Color.red, 0.075f)); // Flash red
                    }
                    else if (hit.collider.tag == "NormalEnemy")
                    {
                        hit.collider.GetComponent<NormalEnemy>().hp -= 2;
                        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[Random.Range(1, 3)]); // Play sound effect
                        hit.collider.GetComponent<NormalEnemy>().lastHit = tag; // Store who gave the last hit so we can update kill counter
                        StartCoroutine(hit.collider.GetComponent<NormalEnemy>().Flash(Color.red, 0.075f)); // Flash red
                    }
                    else if (hit.collider.tag == "ChaserEnemy")
                    {
                        hit.collider.GetComponent<ChaserEnemy>().hp -= 2;
                        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[Random.Range(1, 3)]); // Play sound effect
                        hit.collider.GetComponent<ChaserEnemy>().lastHit = tag; // Store who gave the last hit so we can update kill counter
                        StartCoroutine(hit.collider.GetComponent<ChaserEnemy>().Flash(Color.red, 0.075f)); // Flash red
                    }
                    else if (hit.collider.tag == "ChargerEnemy")
                    {
                        hit.collider.GetComponent<ChargerEnemy>().hp -= 2;
                        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[Random.Range(1, 3)]); // Play sound effect
                        hit.collider.GetComponent<ChargerEnemy>().lastHit = tag; // Store who gave the last hit so we can update kill counter
                        StartCoroutine(hit.collider.GetComponent<ChargerEnemy>().Flash(Color.red, 0.075f)); // Flash red
                    }
                    else if (hit.collider.tag == "Asteroid")
                    {
                        hit.collider.GetComponent<Asteroid>().hp -= 2;
                        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[4]); // Play sound effect
                        hit.collider.GetComponent<Asteroid>().lastHit = tag; // Store who gave the last hit so we can update kill counter
                        StartCoroutine(hit.collider.GetComponent<Asteroid>().Flash(Color.red, 0.075f)); // Flash red
                    }
                }
            }

            yield return wait;
        }

        firing = false;

        transform.GetChild(0).gameObject.SetActive(false);

        while (render.material.color.a > 0)
        {
            Color color = render.material.color;
            color.a -= Time.deltaTime;
            render.material.color = color;
            yield return null;
        }

        Spawn();
        c = null;
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
        GameSystem.instance.soundManager.Stop();
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[0]); // Play sound effect

        GameSystem.instance.enemyList.Remove(gameObject);

        GameObject obj = ObjectPool.instance.GetPooledObject("Orb"); // Spawn orb
        obj.transform.position = transform.position - new Vector3(0.25f, 0);
        obj.transform.rotation = transform.rotation;

        obj = ObjectPool.instance.GetPooledObject("Orb"); // Spawn orb
        obj.transform.position = transform.position + new Vector3(0.25f, 0);
        obj.transform.rotation = transform.rotation;

        obj = ObjectPool.instance.GetPooledObject(explosion); // Spawn explosion
        obj.transform.localScale = explosionSizeVec;
        obj.transform.GetChild(0).localScale = explosionSizeVec;
        obj.transform.GetChild(1).localScale = explosionSizeVec;
        obj.transform.GetChild(2).localScale = explosionSizeVec;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        gameObject.SetActive(false);

        if (lastHit == "Bullet" || lastHit == "RocketExplosion")
        {
            GameSystem.instance.Kills++;
            GameSystem.instance.Score += 50;
        }
    }
}
