using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : SpaceObject
{
    public Orb orb;
    float originalScale;

    // Start is called before the first frame update
    protected override void Awake()
    {
        originalScale = transform.localScale.x; // Since objects shrink when taking damage, store original scale so we can get it back upon respawning
        base.Awake();
    }

    protected override void OnEnable()
    {
        transform.localScale = new Vector3(originalScale, originalScale, 0);
        base.OnEnable();
    }

    protected override void Spawn()
    {
        base.Spawn();
        Vector3 direction = GameSystem.instance.player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Face player when spawning

        transform.position = ChooseLocation();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);

        while (colliders.Length > 0)
        {
            transform.position = ChooseLocation();
            colliders = Physics.OverlapSphere(transform.position, 1);
        }
    }

    protected virtual Vector2 ChooseLocation()
    {
        float xmin = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect + render.bounds.extents.x;
        float xmax = Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect - render.bounds.extents.x;
        float ymin = Camera.main.transform.position.y - Camera.main.orthographicSize + render.bounds.extents.y;
        float ymax = Camera.main.transform.position.y + Camera.main.orthographicSize - render.bounds.extents.y;
        float xposition = Random.Range(xmin, xmax);
        float yposition = Random.Range(ymin, ymax);

        return new Vector2(xposition, yposition);
    }

    protected virtual void FixedUpdate()
    {
        AI();
    }

    protected virtual void AI()
    {
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        transform.localScale -= new Vector3(0.005f, 0.005f, 0); // Shrink object because it looks cool
        base.OnTriggerEnter2D(col);
    }

    protected override void Death()
    {
        GameSystem.instance.enemyList.Remove(gameObject);

        GameObject obj = ObjectPool.instance.GetPooledObject("Orb"); // Spawn orb
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        base.Death();
    }

    public virtual void WaveDeath() // Called when wave ends to destroy all surviving enemies
    {
        base.Death();
    }
}
