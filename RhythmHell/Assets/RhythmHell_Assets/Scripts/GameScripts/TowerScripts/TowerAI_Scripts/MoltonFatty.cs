using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltonFatty : MonoBehaviour {

    [Header("Molton Fatty Variables")]
    public GameObject acidPoolRef;

    private Tower tower;

    private void Awake()
    {
        tower = gameObject.GetComponent<Tower>();
        tower.overRideDeathCall = true;
    }

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
        {
            // Checking for a collision with an Enemy
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.75f, tower.tStats.enemyLayer);
            Debug.DrawRay(transform.position, Vector2.right * 0.75f, Color.black);
            if (hit)
            {
                Enemy enemyHit = hit.collider.gameObject.GetComponent<Enemy>();
                enemyHit.enemyHealth -= tower.tStats.towerDamage;

                GameObject acidPool = Instantiate(acidPoolRef);
                acidPool.transform.position = transform.position;

                GameObject.Destroy(gameObject);
            }

            // Death Call Override
            if (tower.towerHealth <= 0f)
            {
                GameObject acidPool = Instantiate(acidPoolRef);
                acidPool.transform.position = transform.position;

                GameObject.Destroy(gameObject);

                return;
            }
        }
    }

}
