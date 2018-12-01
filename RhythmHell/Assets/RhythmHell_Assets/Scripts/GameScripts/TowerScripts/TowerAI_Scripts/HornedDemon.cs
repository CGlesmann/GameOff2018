using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornedDemon : MonoBehaviour {

    [Header("Tower Stats Ref")]
    public TowerStats tStats;

    [Header("Collided Objects")]
    public List<GameObject> collidedObjects;

    private GameObject target = null;

    private void Awake()
    {
        collidedObjects = new List<GameObject>();
    }

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
        {
            // Checking for a collision
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1f, tStats.enemyLayer);
            if (hit)
            {
                target = hit.collider.gameObject;
                if (!collidedObjects.Contains(target))
                {
                    GetComponent<Tower>().canMove = false;
                    GetComponent<Animator>().SetBool("isAttacking", true);
                }
            }
        }
    }

    public void HitEnemy()
    {
        target.GetComponent<Enemy>().TakeDamage(tStats.towerDamage);
        collidedObjects.Add(target);

        GetComponent<Tower>().canMove = true;
        GetComponent<Animator>().SetBool("isAttacking", false);
    }

}
