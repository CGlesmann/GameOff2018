using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMedic : MonoBehaviour {

    [Header("Medic References")]
    public LayerMask healLayer;
    public EnemyStats eStats;
    public GameObject healTarget = null;

    private float healTimer = 0f;

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
        {
            if (healTarget == null)
            {
                healTarget = GetTarget();
                GetComponent<Enemy>().canMove = true;
            }
            else
            {
                if (healTarget.GetComponent<Enemy>().enemyHealth < healTarget.GetComponent<Enemy>().eStats.maxHealth)
                {
                    GetComponent<Enemy>().canMove = false;
                    // Trying for a heal
                    if (healTimer <= 0f)
                    {
                        // Creating the Bullet
                        GameObject newBullet = Instantiate(eStats.rangeAttackPrefab);
                        EnemyBullet eBullet = newBullet.GetComponent<EnemyBullet>();

                        eBullet.eStats = eStats;
                        eBullet.transform.position = transform.position + eStats.bulletPositionOffset;
                        eBullet.bulletLane = GetComponent<Enemy>().enemyLane;
                        eBullet.bulletParent = gameObject;
                        eBullet.healTarget = healTarget;

                        healTimer = eStats.enemyAttackSpeed;
                    }
                    else
                    {
                        // Decrementing The Timer
                        healTimer -= Time.deltaTime;
                    }
                }
                else
                {
                    healTarget = null;
                }
            }
        }
    }

    private GameObject GetTarget()
    {
        GameObject newTarget = null;
        float newTargetHealth = 99999999999f;

        RaycastHit2D[] targets = Physics2D.RaycastAll(transform.position, eStats.shootAngle, eStats.enemyRange, eStats.enemyLayer);
        foreach(RaycastHit2D target in targets)
        {
            Enemy t = target.collider.gameObject.GetComponent<Enemy>();
            if (t.enemyHealth < newTargetHealth)
            {
                newTarget = t.gameObject;
                newTargetHealth = t.enemyHealth;
            }
        }

        return newTarget;
    }

    private void PlaySound(AudioClip aClip, float delay = 0f)
    {
        GetComponent<Enemy>().aSource.clip = aClip;
        GetComponent<Enemy>().aSource.PlayDelayed(delay);
    }
}
