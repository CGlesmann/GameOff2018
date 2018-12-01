using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    [Header("Bullet Variables")]
    public EnemyStats eStats;
    public GameObject bulletParent;
    public GameObject healTarget = null;
    public int bulletLane;

    [Header("Bullet Sounds")]
    public AudioClip bulletHit;


    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
        {
            // Checking for a collision Ahead
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, 0.5f, eStats.enemyLayer);
            foreach(RaycastHit2D hit in hits)
            {
                if (healTarget == null || (healTarget != null && hit.collider.gameObject == healTarget)) {
                    if (hit.collider.gameObject != bulletParent)
                    {
                        if (eStats.eTarget == EnemyStats.EnemyTarget.Towers)
                        {
                            Tower enemy = hit.collider.gameObject.GetComponent<Tower>();

                            if (enemy.towerLane == bulletLane)
                            {
                                enemy.TakeDamage(eStats.enemyDamage);

                                if (eStats.splashRadius > 0f)
                                {
                                    RaycastHit2D[] towers = Physics2D.CircleCastAll(transform.position, eStats.splashRadius, Vector2.zero, 0f, eStats.enemyLayer);
                                    foreach (RaycastHit2D towerHit in towers)
                                    {
                                        Tower tower = towerHit.collider.gameObject.GetComponent<Tower>();
                                        tower.TakeDamage(eStats.enemyDamage * 0.75f);
                                    }
                                }

                                if (bulletHit != null)
                                {
                                    GetComponent<AudioSource>().clip = bulletHit;
                                    GetComponent<AudioSource>().Play();
                                }

                                GameObject.Destroy(gameObject);
                            }
                        }

                        if (eStats.eTarget == EnemyStats.EnemyTarget.Enemies)
                        {
                            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();

                            if (enemy.enemyLane == bulletLane)
                            {
                                enemy.TakeDamage(eStats.enemyDamage);

                                if (eStats.splashRadius > 0f)
                                {
                                    RaycastHit2D[] towers = Physics2D.CircleCastAll(transform.position, eStats.splashRadius, Vector2.zero, 0f, eStats.enemyLayer);
                                    foreach (RaycastHit2D towerHit in towers)
                                    {
                                        Enemy tower = towerHit.collider.gameObject.GetComponent<Enemy>();
                                        tower.TakeDamage(eStats.enemyDamage * 0.75f);
                                    }
                                }

                                if (bulletHit != null)
                                {
                                    GetComponent<AudioSource>().clip = bulletHit;
                                    GetComponent<AudioSource>().Play();
                                }
                                
                                GameObject.Destroy(gameObject);
                            }
                        }
                    }
                }
            }

            // Moving the Bullet
            transform.position += (eStats.bulletMoveVector * Time.deltaTime);

            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.tag == "EndLine")
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, eStats.splashRadius);
    }
}
