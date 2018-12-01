using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Images
using TMPro; // TextMeshProUGUI Elements

public class RhythmGameManager : MonoBehaviour {

    [Header("Manager References")]
    public GameManager manager; // Reference To Global GameManager
    public EnemySpawner eSpawner; // Reference To Enemy Spawner
    public RhythmSpawner rSpawner; // Reference To Rhythm Spawner

    [Header("Rhythm Game Variables")]
    public bool rGameActive = false;
    public float rgMult = 1f; // Current Multiplier from Rhythm Game
    public float rgMultMin = 1f; // Minimum Multiplier from Rhythm Game
    public float rgMultMax = 2.5f; // Maximum Multiplier from Rhythm Game

    public float incDelay = 0.5f;
    public float rgIncMin = 0f; // Minimum Increment Recieved from hitting Note
    public float rgIncMax = 0.15f; // Maximum Increment Recieved from hitting Note

    public float rgMinusInc = -0.5f;

    [Header("GUI References")]
    public Image rgMultBar; // Reference To Filled Multiplier Bar
    public TextMeshProUGUI multText;

    private float incTimer = 0f;

    private void Awake()
    {
        // Setting rgMult to Min Value
        rgMult = rgMultMin;
    }

    private void Update()
    {
        if (rGameActive && !manager.gamePaused)
        {
            // updating the GUI
            UpdateGUI();

            // Decrementing the rgMult
            DecrementMult(Time.deltaTime);

            return;
        }
    }

    public void StartGame()
    {
        rGameActive = true;
    }

    // Called From RhythmSpawner
    public void NoteHit(float accuracy = 1f)
    {
        // Getting the Increment Based on Accuracy
        float inc = Mathf.Clamp(rgIncMax * accuracy, rgIncMin, rgIncMax);

        // Incrementing rgMult by inc
        rgMult = Mathf.Clamp(rgMult + inc, rgMultMin, rgMultMax);

        incTimer = incDelay;

        return;
    }

    private void DecrementMult(float inc)
    {
        // Decrement rgMult by Inc amount
        if (incTimer <= 0f)
        {
            rgMult = Mathf.Clamp(rgMult + rgMinusInc, rgMultMin, rgMultMax);
            incTimer = incDelay;
        }
        else
            incTimer -= Time.deltaTime;

        return;
    }

    // Called From Update Method
    private void UpdateGUI()
    {
        // Updating rgMultBar
        float fill = (rgMult - rgMultMin) / (rgMultMax - rgMultMin);
        rgMultBar.fillAmount = fill;

        // Updating Mult Text
        multText.text = rgMult.ToString("F1") + "x";

        return;
    }

}
