using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Wave", menuName = "Enemy Assets/Create Enemy Wave", order = 2)]
public class EnemyWave : ScriptableObject {

    [Header("Wave Variables")]
    public GameObject[] enemies;
    public float spawnDelay = 1f;
    public float waveDelay = 0f;
}
