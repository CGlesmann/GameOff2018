using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine_Spawner : MonoBehaviour {

    [Header("Spawner Variables")]
    public GameObject mineReference;
    public float spawnCooldown;

    private float spawnTimer;

    private void Awake()
    {
        spawnTimer = spawnCooldown;
    }

    private void Update()
    {
        if (spawnTimer > 0f)
        {
            spawnTimer -= Time.deltaTime;
            return;
        }

        GameObject newMine = Instantiate(mineReference);
        newMine.transform.position = transform.position;

        spawnTimer = spawnCooldown;
        return;
    }

}
