using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour {

    [Header("Sound Player Variables")]
    public AudioClip playClip;

    private GameManager manager;
    private AudioSource aSource;
    private bool playing = false;

    public void SetUp()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        aSource = GetComponent<AudioSource>();

        aSource.clip = playClip;
        StartCoroutine("WaitForDeath");
        aSource.Play();

        playing = true;
    }

    private void Update()
    {
        if (manager.gamePaused && playing)
        {
            aSource.Pause();
            playing = false;
        } else if (!manager.gamePaused && !playing)
        {
            aSource.Play();
            playing = true;
        }
    }

    IEnumerator WaitForDeath()
    {
        float time = aSource.clip.length;
        Debug.Log("Time: " + time);

        yield return new WaitForSeconds(time);
        GameObject.Destroy(gameObject);
    }

}
