using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SpaceObject
{
    Renderer shieldRender;
    Coroutine rechargeShield;
    float lastBulletTime, lastRocketTime;
    public bool takeDamage = false;
    GameSystem.GameState previousState;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        lastBulletTime = -1; // Set to -1 otherwise we'll have to wait before allowed to fire
        lastRocketTime = -1;
        shieldRender = transform.GetChild(0).GetComponent<Renderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        shieldRender.material.color = new Color(1, 1, 1, 0);
    }

    protected void Start()
    {
        explosion = "Explosion";
    }

    protected override void Update()
    {
        if (GameSystem.instance.gameState == GameSystem.GameState.Wave || GameSystem.instance.gameState == GameSystem.GameState.PauseMenu || GameSystem.instance.gameState == GameSystem.GameState.Shop) // KeyDown doesn't work well with fixed update so we need to do it here
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameSystem.instance.gameState != GameSystem.GameState.PauseMenu)
                {
                    GameSystem.instance.UI.transform.Find("PauseMenu").gameObject.SetActive(true);
                    previousState = GameSystem.instance.gameState; // Store previous state so we can get back to it when pressing Escape again
                    GameSystem.instance.gameState = GameSystem.GameState.PauseMenu;
                }
                else
                {
                    GameSystem.instance.UI.transform.Find("PauseMenu").gameObject.SetActive(false);
                    GameSystem.instance.gameState = previousState;
                }
            }
        }

        base.Update();
    }

    private void FixedUpdate()
    {
        // Player input
        PlayerInput();

        if (takeDamage)
        {
            DecreaseHealthShield(10);
            takeDamage = false;
        }
    }

    protected override IEnumerator FadeIn()
    {
        Color c = render.material.color;
        Color shieldC = shieldRender.material.color;

        while (c.a < 1)
        {
            c.a += 0.005f;

            if (shieldC.a < 1f)
                shieldC.a += 0.005f;

            render.material.color = c;
            shieldRender.material.color = shieldC;
            yield return null;
        }
    }

    private void PlayerInput()
    {
        if (GameSystem.instance.gameState != GameSystem.GameState.PauseMenu) // If not in pause menu, allow movement
        {
            // Mouse input
            // Have ship orientate towards mouse
            Vector2 mouse_pos = Input.mousePosition;
            Vector2 ship_pos = Camera.main.WorldToScreenPoint(transform.position);
            mouse_pos = mouse_pos - ship_pos;
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Shoot bullets
            if (Input.GetMouseButton(0)) // If left mouse button
            {
                if (Time.time - lastBulletTime > 0.2f) // 0.2f is fire rate, which is 1/5 so 5 bullets per second
                {
                    GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.PlayerSounds   [Random.Range(0, 3)]); // Play sound effect
                    lastBulletTime = Time.time;
                    bullet = ObjectPool.instance.GetPooledObject("Bullet");

                    if (bullet != null)
                    {
                        bullet.transform.position = transform.position;
                        bullet.transform.rotation = transform.rotation;
                        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                        bullet.GetComponent<Bullet>().SetDirection(direction);
                    }
                }
            }

            if (GameSystem.instance.gameState != GameSystem.GameState.Shop) // If in shop, disable some controls
            {
                // Shoot rockets
                if (Input.GetMouseButton(1)) // If right mouse button
                {
                    if (GameSystem.instance.Rockets > 0 && Time.time - lastRocketTime > 1f) // 1f is fire rate, which is 1/1 so 1 rocket per second
                    {
                        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.PlayerSounds[Random.Range(4, 6)]); // Play sound effect
                        GameSystem.instance.Rockets--;
                        lastRocketTime = Time.time;

                        GameObject rocket = ObjectPool.instance.GetPooledObject("Rocket");

                        if (rocket != null)
                        {
                            rocket.transform.position = transform.position;
                            rocket.transform.rotation = transform.rotation;
                            Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                            rocket.GetComponent<Rocket>().SetDirection(direction);
                        }
                    }
                }

                // Keyboard input
                if (transform.position.x - 0.5f < Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect) // If crossing left boundary, stop and push back
                {
                    body.velocity = new Vector2(0, body.velocity.y);
                    body.AddForce(new Vector2(5, 0));
                }
                else if (Input.GetKey(KeyCode.A)) // Else move to the left
                    body.AddForce(new Vector2(-10, 0));

                if (transform.position.y + 0.5f > Camera.main.transform.position.y + Camera.main.orthographicSize) // Up movement handling
                {
                    body.velocity = new Vector2(body.velocity.x, 0);
                    body.AddForce(new Vector2(0, -5));
                }
                else if (Input.GetKey(KeyCode.W))
                    body.AddForce(new Vector2(0, 10));

                if (transform.position.y - 0.5f < Camera.main.transform.position.y - Camera.main.orthographicSize) // Down movement handling
                {
                    body.velocity = new Vector2(body.velocity.x, 0);
                    body.AddForce(new Vector2(0, 5));
                }
                else if (Input.GetKey(KeyCode.S))
                    body.AddForce(new Vector2(0, -10));

                if (transform.position.x + 0.5f > Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect) // Right movement handling
                {
                    body.velocity = new Vector2(0, body.velocity.y);
                    body.AddForce(new Vector2(-5, 0));
                }
                else if (Input.GetKey(KeyCode.D))
                    body.AddForce(new Vector2(10, 0));
            }
        }
    }

    public IEnumerator IgnoreExplosion(Collider2D rocketExplosion) // Ignore any rocket explosion that detonated more than 0.2 seconds ago
    {
        yield return new WaitForSeconds(0.2f);
        rocketExplosion.enabled = false;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Bullet" && col.tag != "Rocket") // Make sure player doesn't die from its own bullets/rockets
        {
            GameObject obj = ObjectPool.instance.GetPooledObject(explosion); // Spawn explosion
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 1);
            obj.transform.GetChild(0).localScale = new Vector3(0.05f, 0.05f, 1);
            obj.transform.GetChild(1).localScale = new Vector3(0.05f, 0.05f, 1);
            obj.transform.GetChild(2).localScale = new Vector3(0.05f, 0.05f, 1);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            StartCoroutine(Flash(Color.red, 0.075f)); // Flash red

            if (col.tag == "NormalEnemyBullet")
            {
                GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.PlayerSounds[8]); // Play sound effect
                col.gameObject.SetActive(false);
                DecreaseHealthShield(10);
            }

            if (col.tag == "LaserEnemyBullet")
            {
                col.gameObject.SetActive(false);
                DecreaseHealthShield(2);
            }
        }

        if (col.tag == "Orb") // If picking up orb
        {
            GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.PlayerSounds[9]); // Play sound effect
            GameSystem.instance.Orbs++;
            StartFade(Color.green, 2);
            col.gameObject.SetActive(false);
        }
    }

    public void DecreaseHealthShield(int damage)
    {
        if (GameSystem.instance.Shield > 0) // If shield, decrease shield
        {
            GameSystem.instance.Shield -= damage;
            if (GameSystem.instance.Shield < 0) // If shield drops below 0, set to 0 and subtract the difference from health
            {
                hp += GameSystem.instance.Shield;
                GameSystem.instance.Shield = 0;
            }
        }
        else // Otherwise, decrease health
            hp -= damage;

        GameSystem.instance.Health = hp; // Update UI health
        shieldRender.material.SetColor("_Color", new Color(1, 1, 1, (float)GameSystem.instance.Shield / GameSystem.instance.shieldCapacity)); // Make shield less visible

        if (rechargeShield != null)
            StopCoroutine(rechargeShield);

        rechargeShield = StartCoroutine(RechargeShield());
    }

    private IEnumerator RechargeShield() // Quickly recharge shield to full after not taking damage for shieldTime seconds
    {
        yield return new WaitForSeconds(GameSystem.instance.shieldTime);

        while (GameSystem.instance.Shield < GameSystem.instance.shieldCapacity) // Rapidly recharge shield
        {
            yield return new WaitForSeconds(0.05f);
            GameSystem.instance.Shield++;
            shieldRender.material.SetColor("_Color", new Color(1, 1, 1, (float)GameSystem.instance.Shield / GameSystem.instance.shieldCapacity));

            if (GameSystem.instance.Shield > GameSystem.instance.shieldCapacity) // If it overshoots
                GameSystem.instance.Shield = GameSystem.instance.shieldCapacity;
        }
    }

    protected override void Death()
    {
        GameSystem.instance.soundManager.PlayOneShot(GameSystem.instance.EnemySounds[0]); // Play sound effect
        GameSystem.instance.Lives--; // Update UI lives

        if (GameSystem.instance.Lives > 0)
            GameSystem.instance.StartACoroutine(GameSystem.instance.DeathCountdown());
        else
            GameSystem.instance.gameState = GameSystem.GameState.GameOver;

        base.Death();
    }
}
