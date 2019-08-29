using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Enemy
{
    Camera cam;
    int speed;
    int rotationSpeed;
    bool immunity;
    Vector3 scale;

    protected override void Awake()
    {
        base.Awake();

        scale = transform.localScale;
        explosion = "AsteroidExplosion";
    }

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();
        hp = 50;
        immunity = true;
        StartCoroutine(TemporaryImmunity()); // Grant asteroid 1 second of immunity so it doesn't destroy itself upon entering the scene (see AI() method)
        cam = Camera.main;
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + Camera.main.orthographicSize, 0);
        transform.localScale = scale * Random.Range(0.8f, 1.6f); // Choose a random size

        // Choose random rotation speed
        rotationSpeed = 0;
        while (rotationSpeed == 0)
            rotationSpeed = Random.Range(-4, 4);

        // Move through scene
        speed = Random.Range(3, 8); // Choose random speed
        int side = Random.Range(0, 3); // Choose random side of camera from which to enter scene
        float sidePosition = Random.Range(-4f, 4f) * 0.2f; // Spawn on random position on side
        float sideDestination = Random.Range(-4f, 4f) * 0.2f; // Move to random destination on other side
        float xbounds = render.bounds.extents.x;
        float ybounds = render.bounds.extents.y;

        Vector2 sideDest;

        if (side == 0)
        {
            transform.position = new Vector2(Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect - xbounds, Camera.main.transform.position.y + sidePosition * Camera.main.orthographicSize);
            sideDest = new Vector2(Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect + xbounds, Camera.main.transform.position.y + sideDestination * Camera.main.orthographicSize);
            body.velocity = (sideDest - (Vector2)transform.position).normalized * speed;
        }
        else if (side == 1)
        {
            transform.position = new Vector2(Camera.main.transform.position.x + sidePosition * Camera.main.orthographicSize * Camera.main.aspect, Camera.main.transform.position.y + Camera.main.orthographicSize + ybounds);
            sideDest = new Vector2(Camera.main.transform.position.x + sideDestination * Camera.main.orthographicSize * Camera.main.aspect, Camera.main.transform.position.y - Camera.main.orthographicSize - ybounds);
            body.velocity = (sideDest - (Vector2)transform.position).normalized * speed;
        }
        else if (side == 2)
        {
            transform.position = new Vector2(Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect + xbounds, Camera.main.transform.position.y + sidePosition * Camera.main.orthographicSize);
            sideDest = new Vector2(Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect - xbounds, Camera.main.transform.position.y + sideDestination * Camera.main.orthographicSize);
            body.velocity = (sideDest - (Vector2)transform.position).normalized * speed;
        }
        else if (side == 3)
        {
            transform.position = new Vector2(Camera.main.transform.position.x + sidePosition * Camera.main.orthographicSize * Camera.main.aspect, Camera.main.transform.position.y - Camera.main.orthographicSize - ybounds);
            sideDest = new Vector2(Camera.main.transform.position.x + sideDestination * Camera.main.orthographicSize * Camera.main.aspect, Camera.main.transform.position.y + Camera.main.orthographicSize + ybounds);
            body.velocity = (sideDest - (Vector2)transform.position).normalized * speed;
        }
    }

    private IEnumerator TemporaryImmunity()
    {
        yield return new WaitForSeconds(1);
        immunity = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void AI() // Program asteroid AI
    {
        transform.Rotate(Vector3.forward, rotationSpeed); // Rotate asteroid because it looks cool

        if (!GameSystem.instance.InView(gameObject) && !immunity) // If not visible on cameras and not recently spawned, destroy
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        int damage = speed * 10; // Set damage based on asteroid speed
        col.gameObject.GetComponent<Rigidbody2D>().AddForce((col.collider.ClosestPoint(transform.position) - (Vector2)transform.position).normalized * 300); // Use amazing linear algebra to push object away from asteroid
        SpaceObject obj = col.gameObject.GetComponent<SpaceObject>();
        obj.StartCoroutine(obj.Flash(Color.red, 0.075f));

        if (col.gameObject.tag == "Player") // If hitting player
        {
            GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.PlayerSounds[8]); // Play sound effect
            Player player = col.gameObject.GetComponent<Player>();
            player.DecreaseHealthShield(damage);
        }
        else
        {
            col.gameObject.GetComponent<SpaceObject>().hp -= damage;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[4]); // Play sound effect
        base.OnTriggerEnter2D(col);
    }

    protected override void Death()
    {
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[9]); // Play sound effect

        GameSystem.instance.enemyList.Remove(gameObject);

        GameObject obj = ObjectPool.instance.GetPooledObject("Orb"); // Spawn orb
        obj.transform.position = transform.position - new Vector3(0.5f, 0);
        obj.transform.rotation = transform.rotation;

        obj = ObjectPool.instance.GetPooledObject("Orb"); // Spawn orb
        obj.transform.position = transform.position + new Vector3(0.5f, 0);
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
            GameSystem.instance.Score += 80;
        }
    }
}
