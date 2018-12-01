using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerInfoWindow : MonoBehaviour {

    public TowerStats towerStats;
    public Vector3 positionOffset;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;

    private void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(pos.x + positionOffset.x, pos.y + positionOffset.y, 0f);

        // Updating the GUI
        UpdateGUI();
    }

    private void UpdateGUI()
    {
        nameText.text = towerStats.towerName;                    
        costText.text = "$" + towerStats.towerCost.ToString("F0");
    }

}
