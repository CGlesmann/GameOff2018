using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    [Header("Bullet Variables")]
    public int bulletLane;
    public float attackMult;
    [HideInInspector] public TowerStats tStats;

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
        {
            // Checking for a collision Ahead
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.5f, tStats.enemyLayer);
            if (hit)
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemy.enemyLane == bulletLane)
                {
                    enemy.TakeDamage(tStats.towerDamage * attackMult);

                    GameObject.Destroy(gameObject);
                }
            }

            // Moving the Bullet
            transform.position += (tStats.bulletMoveVector * Time.deltaTime);

            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.tag == "StartLine")
        {
            GameObject.Destroy(gameObject);
        }
    }
}
