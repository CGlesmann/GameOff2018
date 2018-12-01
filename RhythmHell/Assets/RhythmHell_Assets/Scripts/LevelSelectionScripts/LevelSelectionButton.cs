using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionButton : MonoBehaviour {

    [Header("GUI References")]
    public GameObject selectionArrow;

    private void OnMouseEnter()
    {
        if (!selectionArrow.activeSelf)
            selectionArrow.SetActive(true);

        selectionArrow.transform.position = new Vector3(selectionArrow.transform.position.x, transform.position.y, 0f);
    }

    private void OnMouseExit()
    {
        selectionArrow.SetActive(false);
    }

}
