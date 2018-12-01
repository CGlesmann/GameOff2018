using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mine : MonoBehaviour {

    [Header("Mine Variables")]
    public LayerMask enemyLayer;
    public float mineCooldown;
    public float mineDamage;
    public Vector3 explosionRadius;

    [Header("Mine Sounds")]
    public AudioClip minePlaced;
    public AudioClip mineTick;
    public AudioClip mineExplosion;

    private float mineTimer;

    private void Awake()
    {
        // Setting the Mine Timer
        mineTimer = mineCooldown;

        // Placed Sound 
        GetComponent<AudioSource>().clip = minePlaced;
        GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        if (mineTimer > 0f)
        {
            mineTimer -= Time.deltaTime;

            // Placed Sound 
            if (GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().clip = mineTick;
                GetComponent<AudioSource>().Play();
            }

            if (mineTimer <= 0f)
            {
                // destroy The Mine
                DestroyMine();
            }
        }
    }

    private void DestroyMine()
    {
        // Placed Sound 
        if (GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = mineExplosion;
            GetComponent<AudioSource>().Play();
        }

        // Getting all units in explosion Radius
        RaycastHit2D[] targets = Physics2D.BoxCastAll(transform.position, explosionRadius, 0f, Vector2.zero, 0f, enemyLayer);
        foreach(RaycastHit2D target in targets)
        {
            Tower targetTower = target.collider.gameObject.GetComponent<Tower>();
            targetTower.TakeDamage(mineDamage);
        }

        GameObject.Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, explosionRadius);
    }

}
