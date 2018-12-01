using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTowerIcon : MonoBehaviour {

    [Header("GUI References")]
    public GameObject iconHighlighter;

    [Header("Tower Build Options")]
    public GameManager manager;
    public GameObject tower;
    public GameObject towerInfoTooltip;

    public void SetBuildTower()
    {
        if (manager.money >= 50f)
        {
            if (manager.buildTower == tower)
            {
                manager.buildTower = null;
                if (iconHighlighter != null)
                    iconHighlighter.SetActive(false);

                manager.towerPlacer.gameObject.SetActive(false);
            }
            else
            {
                manager.buildTower = tower;
                if (iconHighlighter != null)
                    iconHighlighter.SetActive(true);

                Vector3 mousePos = Input.mousePosition;
                Vector3 translatedMousePos = Camera.main.ScreenToWorldPoint(mousePos);

                manager.towerPlacer.gameObject.transform.position = new Vector3(translatedMousePos.x, translatedMousePos.y, 0f);
                manager.towerPlacer.gameObject.SetActive(true);
                manager.towerPlacer.SetUpPlacer(tower);
            }
        }
    }

    private void Update()
    {
        if (manager.buildTower != tower)
        {
            if (iconHighlighter != null)
                iconHighlighter.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        towerInfoTooltip.SetActive(true);
        towerInfoTooltip.GetComponent<TowerInfoWindow>().towerStats = tower.GetComponent<Tower>().tStats;
    }

    private void OnMouseExit()
    {
        towerInfoTooltip.SetActive(false);
        towerInfoTooltip.GetComponent<TowerInfoWindow>().towerStats = null;
    }

}
