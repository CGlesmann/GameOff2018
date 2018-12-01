using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {

    private static MusicPlayer mPlayer = null;

    [Header("Do Not Play List")]
    public List<string> levelNames;

    private void Awake()
    {
        if (mPlayer != null)
            Object.Destroy(gameObject);
        else
            mPlayer = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        string level = SceneManager.GetActiveScene().name;
        if (levelNames.Contains(level))
        {
            GetComponent<AudioSource>().Pause();
        }

        if (!GetComponent<AudioSource>().isPlaying)
        {
            if (!levelNames.Contains(level))
            {
                GetComponent<AudioSource>().Play();
            }
        }
    }

}
