using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    [Header("Sound Refs")]
    public AudioClip clickSound;

    public void GoToNewScene(string sceneName)
    {
        if (clickSound != null)
        {
            GetComponent<AudioSource>().clip = clickSound;
            GetComponent<AudioSource>().Play();
        }

        SceneManager.LoadScene(sceneName);
    }

}
