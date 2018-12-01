using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour {

    [Header("Acid Pool Variables")]
    public LayerMask enemyLayer;
    [Tooltip("Damage to enemy units every second")] public float acidPoolDamage;

    [Tooltip("Amount of time the pool stays active (In Seconds)")] public float acidPoolLifeTime;
    private float acidPoolTimer;

    private void Awake()
    {
        acidPoolTimer = acidPoolLifeTime;
    }

    private void Update()
    {
        acidPoolTimer -= Time.deltaTime;
        if (acidPoolTimer <= 0f)
        {
            GameObject.Destroy(gameObject);
        }

        RaycastHit2D[] enemies = Physics2D.CircleCastAll(transform.position, 1f, Vector2.right, 0f, enemyLayer);
        foreach(RaycastHit2D hit in enemies)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            enemy.enemyHealth -= acidPoolDamage * Time.deltaTime;
        }
    }

}
