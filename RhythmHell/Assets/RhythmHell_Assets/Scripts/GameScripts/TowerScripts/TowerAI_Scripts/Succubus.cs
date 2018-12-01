using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Succubus : MonoBehaviour {

    [Header("Tower Stats Reference")]
    public TowerStats tStats;
    public float stunDelay = 3f;

    private float abilityTimer = 0f;

    private Enemy e;

    private void Update()
    {
        if (abilityTimer <= 0f)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right, tStats.towerRange, tStats.enemyLayer))
                GetComponent<Animator>().SetBool("isAttacking", true);
        } else
        {
            abilityTimer -= Time.deltaTime;
        }
    }

    public void StunEnemies()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, tStats.towerRange, tStats.enemyLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.GetComponent<Enemy>().enemyLane == GetComponent<Tower>().towerLane)
            {
                e = hit.collider.GetComponent<Enemy>();
                e.Stun(stunDelay);
            }
        }

        GetComponent<Animator>().SetBool("isAttacking", false);
        abilityTimer = tStats.towerAttackSpeed;
    }
}
