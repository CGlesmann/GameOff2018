using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour {

    [Header("Manager References")]
    public GameManager manager; // Reference To GameManager
    public RhythmGameManager rManager; // Reference To the Rhythm Game Manager
    public RhythmSpawner rSpawner; // Reference To the Rhythm Game Spawner

    [Header("Wave References")]
    public EnemyWave[] enemyWaves; // An Array of EnemyWave Objects, this is used to track progress
    [HideInInspector] public int currentWave = 0; // The Index tracker for enemyWaves, default 0
    
    [Header("Control Variables")]
    public bool isSpawning = false; // Keeps track of when enemies are spawning

    [Header("Lane References")]
    public GameObject laneParent;

    public void Awake()
    {
        // Setting the Tracking Variables
        isSpawning = false;
        currentWave = 0;
    }

    // Called from The GameManager
    public void StartGame()
    {
        // Allow Spawning to Begin
        isSpawning = true;

        // Begin the Spawning Process
        StartCoroutine("SpawnEnemyWaves");
    }

    IEnumerator SpawnEnemyWaves()
    {
        // Declaring temp var, used to store current wave
        EnemyWave waveToSpawn = null;

        if (!manager.gamePaused)
        {
            // Begin The Wave Spawning Loop
            for (currentWave = 0; currentWave < enemyWaves.Length; currentWave++)
            {
                // Get The Wave To Spawn
                waveToSpawn = enemyWaves[currentWave];

                // Enemy Spawn Loop
                for (int currentEnemy = 0; currentEnemy < waveToSpawn.enemies.Length; currentEnemy++)
                {
                    // Get the Spawning Position
                    int index = Random.Range(0, 4);
                    GameObject lane = laneParent.transform.GetChild(index).gameObject;
                    Vector3 spawnPos = new Vector3(lane.GetComponent<SpriteRenderer>().bounds.max.x + 1.2f, lane.transform.position.y, 0f);

                    // Gets the Enemy Reference and stores it in newEnemy
                    GameObject newEnemy = Instantiate(waveToSpawn.enemies[currentEnemy]);
                    newEnemy.GetComponent<Enemy>().enemyLane = index;

                    // Setting the Enemies Position to spawnPos
                    newEnemy.transform.position = spawnPos;

                    // Calling Enemy Spawn Delay
                    yield return new WaitForSeconds(waveToSpawn.spawnDelay);
                }

                // Calling Wave Spawn Delay
                if (waveToSpawn.waveDelay > 0f)
                {
                    yield return new WaitForSeconds(waveToSpawn.waveDelay);
                }
            }
        } else
        {
            while (manager.gamePaused)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

}
