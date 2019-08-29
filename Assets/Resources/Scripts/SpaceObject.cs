using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SpaceObject : MonoBehaviour
{
    public float explosionSize;
    public int hp = 30;
    protected GameObject bullet;
    protected Transform closestObject;
    protected Renderer render;
    protected float lastTime;
    protected Rigidbody2D body;
    public string lastHit;
    protected string explosion;
    protected GameObject tempRocket;
    protected Vector3 explosionSizeVec;
    Vector2 lastVelocity;
    Coroutine flashCo;
    bool frozen;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        explosionSizeVec = new Vector3(explosionSize, explosionSize, 1);
        lastTime = 0;
        render = GetComponent<Renderer>();
        tempRocket = null;
        frozen = false;
    }

    protected virtual void OnEnable()
    {
        render.material.SetColor("_Color", new Color(1, 1, 1, 0)); // Make invisible
        Spawn();
    }

    protected virtual void Spawn()
    {
        StartCoroutine(FadeIn());
    }

    protected virtual IEnumerator FadeIn() // Fade in when spawning for cool effect
    {
        Color c = render.material.color;

        while (c.a < 1)
        {
            c.a += 0.005f;
            render.material.color = c;
            yield return null;
        }
    }

    protected virtual void Update()
    {
        // If health <= 0, die
        if (hp <= 0)
        {
            Death();
        }

        if (GameSystem.instance.gameState == GameSystem.GameState.PauseMenu) // If game paused, freeze
        {
            if (!frozen)
            {
                lastVelocity = body.velocity;
                body.constraints = RigidbodyConstraints2D.FreezeAll;
                frozen = true;
            }
        }
        else
        {
            if (frozen)
            {
                body.constraints = RigidbodyConstraints2D.None;
                body.velocity = lastVelocity;
                frozen = false;
            }
        }
    }

    public virtual void StartACoroutine(IEnumerator co) // Used to call coroutines from other objects that are about to die so the coroutine doesn't stop when they do
    {
        StartCoroutine(co);
    }

    public virtual void StartFade(Color c, float s) // Method to start Fade coroutine from a different script (if the other GameObject is destroyed, the coroutine won't finish)
    {
        if (flashCo != null) // If coroutine is already playing, stop it so the second Fade works properly
            StopCoroutine(flashCo);

        flashCo = StartCoroutine(Fade(c, s));
    }

    public virtual IEnumerator Fade(Color c, float s) // Fade from color c back to normal in 1 / s seconds
    {
        float t = 0;

        while (t < 1)
        {
            render.material.color = Color.Lerp(c, Color.white, t);
            t += Time.deltaTime * s;

            yield return null;
        }
    }

    public virtual IEnumerator FadeToColor(Color c, float s) // Fade to color c in 1 / s seconds
    {
        float t = 0;
        Color temp = render.material.color;

        while (t < 1)
        {
            render.material.color = Color.Lerp(temp, c, t);
            t += Time.deltaTime * s;

            yield return null;
        }
    }

    public virtual IEnumerator Flash(Color c, float s) // Flash color c for s seconds
    {
        float t = 0;
        render.material.SetColor("_Color", c);

        while (t <= s)
        {
            t += Time.deltaTime;

            if (t >= s)
                render.material.SetColor("_Color", Color.white);

            yield return null;
        }
    }

    protected virtual IEnumerator WaitASec(float t) // Useful for delaying stuff
    {
        yield return new WaitForSeconds(t);
    }

    protected virtual void Death() // Stuff that happens when this object is destroyed
    {
        GameObject obj = ObjectPool.instance.GetPooledObject(explosion); // Spawn explosion
        obj.transform.localScale = explosionSizeVec;
        obj.transform.GetChild(0).localScale = explosionSizeVec;
        obj.transform.GetChild(1).localScale = explosionSizeVec;
        obj.transform.GetChild(2).localScale = explosionSizeVec;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        GameObject obj = ObjectPool.instance.GetPooledObject(explosion); // Spawn explosion
        obj.transform.localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.GetChild(0).localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.GetChild(1).localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.GetChild(2).localScale = new Vector3(0.05f, 0.05f, 1);
        obj.transform.position = GetComponent<Collider2D>().ClosestPoint(col.transform.position);
        obj.transform.rotation = transform.rotation;
        StartCoroutine(Flash(Color.red, 0.075f)); // Flash red

        lastHit = col.gameObject.tag; // Store who gave the last hit so we can update kill counter

        // If hit by bullet
        if (col.tag == "Bullet")
        {
            col.gameObject.SetActive(false); // Destroy bullet
            hp -= 10;
            GameSystem.instance.Score += 5; // Award points for decent aim
        }

        if (col.tag == "NormalEnemyBullet")
        {
            col.gameObject.SetActive(false);
            hp -= 10;
        }

        if (col.tag == "LaserEnemyBullet")
        {
            col.gameObject.SetActive(false);
            hp -= 2;
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
}
