using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant_Skeleton : MonoBehaviour {

    [Header("Giant Skeleton Variables")]
    public GameObject bonePilePrefab;

    private Tower tower;

    private void Awake()
    {
        tower = gameObject.GetComponent<Tower>();
        tower.overRideDeathCall = true;
    }

    private void Update()
    {
        if (tower.towerHealth <= 0f)
        {
            GameObject bones = Instantiate(bonePilePrefab);
            bones.transform.position = transform.position;
            bones.GetComponent<Tower>().towerLane = tower.towerLane;

            GameObject.Destroy(gameObject);

            return;
        }
    }

}
