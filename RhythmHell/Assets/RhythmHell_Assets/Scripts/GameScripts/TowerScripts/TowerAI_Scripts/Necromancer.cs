using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : MonoBehaviour {

    [Header("Tower Stats Reference")]
    public TowerStats tStats;

    [Header("Healing Variables")]
    public LayerMask healLayer;

    private Tower t;

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, tStats.towerRange, healLayer);
            if (hit)
            {
                GetComponent<Animator>().SetBool("isAttacking", true);
            }
        }
    }

    public void HealTower()
    {
        // Getting the Target
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, tStats.towerRange, healLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject != gameObject)
            {
                t = hit.collider.gameObject.GetComponent<Tower>();

                float healAmount = tStats.towerDamage;
                t.towerHealth = Mathf.Clamp(t.towerHealth + (healAmount), 0f, t.towerMaxHealth);
            }
        }

        GetComponent<Animator>().SetBool("isAttacking", false);
    }
}
