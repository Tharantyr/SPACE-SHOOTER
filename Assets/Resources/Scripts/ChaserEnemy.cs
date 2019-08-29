using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        explosion = "NormalEnemyExplosion";
    }

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();
        hp = 10;
    }

    protected override void Spawn()
    {
        base.Spawn();

        Vector3 direction = GameSystem.instance.player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Face player when spawning

        transform.position = ChooseLocation();
        float distanceToPlayer = Vector2.Distance(transform.position, GameSystem.instance.player.transform.position);

        while (distanceToPlayer < 4)
        {
            transform.position = ChooseLocation();
            distanceToPlayer = Vector2.Distance(transform.position, GameSystem.instance.player.transform.position);
        }
    }

    protected override void AI() // Program chaser enemy AI
    {
        if (GameSystem.instance.player != null) // Make sure there's still something left to kill
        {
            // Orientate towards player
            Vector3 direction = GameSystem.instance.player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), 5 * Time.deltaTime);

            // Move closer to player
            if (Vector2.Distance(transform.position, GameSystem.instance.player.transform.position) > 0)
                GetComponent<Rigidbody2D>().AddForce(direction.normalized * 10);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[Random.Range(1, 3)]); // Play sound effect

        GameObject obj = ObjectPool.instance.GetPooledObject(explosion); // Spawn explosion
        obj.transform.localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.GetChild(0).localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.GetChild(1).localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.GetChild(2).localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.position = GetComponent<Collider2D>().ClosestPoint(col.transform.position);
        obj.transform.rotation = transform.rotation;

        if (gameObject.activeInHierarchy)
            StartCoroutine(Flash(Color.red, 0.075f)); // Flash red

        lastHit = col.gameObject.tag; // Store who gave the last hit so we can update kill counter

        if (col.gameObject.tag == "Player")
        {
            GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[0]); // Play sound effect
            Player player = col.gameObject.GetComponent<Player>();
            player.DecreaseHealthShield(10);
            WaveDeath();
        }

        // If hit by bullet
        if (col.tag == "Bullet")
        {
            col.gameObject.SetActive(false); // Destroy bullet
            hp -= 10;
            GameSystem.instance.Score += 5; // Award points for decent aim
        }

        if (col.tag == "Rocket") // If rocket collision, detonate rocket
        {
            GameObject rocket = ObjectPool.instance.GetPooledObject("RocketExplosion"); // Create rocket explosion
            rocket.transform.position = GetComponent<Collider2D>().ClosestPoint(col.transform.position);
            rocket.transform.rotation = transform.rotation;
            rocket.GetComponent<Collider2D>().enabled = true;
            tempRocket = col.gameObject;
            tempRocket.SetActive(false);
        }

        if (col.tag == "RocketExplosion") // If hit by rocket explosion
        {
            GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.PlayerSounds[7]); // Play sound effect
            hp -= 30;
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.StartACoroutine(player.IgnoreExplosion(col));
            GameSystem.instance.Score += 15;
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
}
