using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TowerTile : MonoBehaviour {

    [Header("Tile Variables")]
    public GameManager manager;

    public Color unselected_tile;
    public Color selected_tile;

    private bool towerPlaced = false;
    private SpriteRenderer sr;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        sr = GetComponent<SpriteRenderer>();
        sr.color = unselected_tile;

        towerPlaced = false;
    }

    private void OnMouseOver()
    {
        if (manager.buildTower != null)
            sr.color = selected_tile;
    }

    private void OnMouseExit()
    {
        sr.color = unselected_tile;
    }

    private void OnMouseDown()
    {
        if (manager.buildTower != null)
        {
            if (manager.money >= 50f)
            {
                if (!towerPlaced)
                {
                    GameObject newTower = Instantiate(manager.buildTower);
                    newTower.transform.position = transform.position;

                    towerPlaced = true;
                    manager.money -= 50;

                    if (manager.money <= 0)
                    {
                        manager.buildTower = null;
                    }
                }
            }
        }
    }

}
