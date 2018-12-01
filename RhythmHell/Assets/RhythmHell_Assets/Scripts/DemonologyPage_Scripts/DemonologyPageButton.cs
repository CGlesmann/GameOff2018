using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DemonologyPageButton : MonoBehaviour {

    [Header("Page Manager Reference")]
    public DemonologyPageManager dManager;

    [Header("Button Variables")]
    public TowerStats tStats;

    private void OnMouseDown()
    {
        if (dManager.buttonReference == tStats)
        {
            dManager.buttonReference = null;
            dManager.DeactivatePage();
        } else
        {
            dManager.buttonReference = tStats;
            dManager.ActivatePage();
        }
    }

}
