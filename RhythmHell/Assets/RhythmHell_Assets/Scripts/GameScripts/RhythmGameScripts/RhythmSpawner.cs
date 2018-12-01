using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmSpawner : MonoBehaviour {

    [Header("Rhythm Tool Variables")]
    public RhythmTool rhythmTool; // RhythmTool Ref
    public RhythmEventProvider rhythmToolEventProvider; // Event Provider Ref

    [Header("Other References")]
    public GameObject rhythmKey; // The Reference to RhythmKey Prefab
    public AudioClip anaylize_audioClip; // The AudioClip to analysis for spawning notes
    public AudioClip play_audioClip; // The AudioClip to play out loud

    [Header("Spawner Onset Range")]
    public int frameOffset; // The Amount of frames to offset Onset Detection By

    [Header("Misc. Variables")]
    public LayerMask noteMask;
    public Vector3 spawnerCollisionRadius;
    public int spawnChance = 3;
    [HideInInspector] public float rhythmKeyTravelTime = 1f;

    private bool isSpawning = false; // Used to track when song is playing

    private void Awake()
    {
        // Setting the Travel Time
        RhythmKey rKey = rhythmKey.GetComponent<RhythmKey>();
        if (rKey.mAxis == RhythmKey.MoveAxis.X)
            rhythmKeyTravelTime = Mathf.Abs(rKey.destroyPoint.x - rKey.startPoint.x) / Mathf.Abs(rKey.moveSpeed.x);
        if (rKey.mAxis == RhythmKey.MoveAxis.Y)
            rhythmKeyTravelTime = Mathf.Abs(rKey.destroyPoint.y - rKey.startPoint.x) / Mathf.Abs(rKey.moveSpeed.y);

        // Stops RhythmTool from Playing at start (Allows for Countdown)
        rhythmTool.Stop();

        // Turns off Spawning
        isSpawning = false;

        return;
    }

    // Called When The Level Is Beginning
    public void StartGame()
    {
        // Loading the Provided Song
        rhythmToolEventProvider.SongLoaded += OnSongLoaded;

        // Setting the Song
        rhythmTool.audioClip = anaylize_audioClip;

        //Enabling Spawning
        isSpawning = true;

        return;
    }

    public void PauseGame()
    {
        rhythmTool.Pause();

        isSpawning = false;
    }

    public void UnPauseGame()
    {
        rhythmTool.Play();

        isSpawning = true;
    }

    public void Update()
    {
        // Checking for notes to be spawned (Set in StartGame)
        if (isSpawning && SpawnerFree())
        {
            // Declaring Analysis Variables
            AnalysisData low = rhythmTool.low; // Stores the Data from the loaded song
            Onset onset_low; // reference for AnalysisData onSets

            // Checking for an onset (note) at given frame, Stores result in onset_low
            if (low.onsets.TryGetValue(rhythmTool.currentFrame + Mathf.RoundToInt(rhythmKeyTravelTime * 30) - frameOffset, out onset_low))
            {
                int spawn = Random.Range(1, spawnChance + 1);

                if (spawn == spawnChance)
                {
                    // If Onset found, spawn A RhythmKey
                    Instantiate(rhythmKey);
                }
            }
        }

        return;
    }
    
    // Called When Subscribing To OnSongLoaded Event (rhythmToolEventProvider)
    private void OnSongLoaded()
    {
        // Feeding RhythmTool the Song to play/analysis
        rhythmTool.Play(play_audioClip, anaylize_audioClip, 5f);
    }

    // Checks to see if the spawner is clear of notes
    private bool SpawnerFree()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, spawnerCollisionRadius, 0f, Vector2.zero, 0f, noteMask);
        return (hits.Length == 0);
    }

    // Draws the spawner Collision Radius
    private void OnDrawGizmosSelected()
    {
        // Drawing a Cube
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position, spawnerCollisionRadius);
    }

}
