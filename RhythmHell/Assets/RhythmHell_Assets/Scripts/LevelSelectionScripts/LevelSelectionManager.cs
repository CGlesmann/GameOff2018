using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour {

    [Header("Other Screen References")]
    public string demonologyPage;
    public string mainMenuPage;

    [Header("Level Names")]
    public string[] levelNames; // Holds a reference to all scene names

    // Called from LevelSelectionButtons, passes in index for levelNames
    public void GoToLevel(int levelIndex = 0)
    {
        // Validating levelIndex
        if (levelIndex < 0 || levelIndex > levelNames.Length - 1)
        {
            Debug.Log("levelIndex isn't properly set");
            return;
        }

        // Getting the Level String
        string levelName = levelNames[levelIndex];

        // Loading the Scene
        SceneManager.LoadScene(levelName);
    }

    public void GoToDemonology()
    {
        SceneManager.LoadScene(demonologyPage);
    }

    public void GoToMainScreen()
    {
        SceneManager.LoadScene(mainMenuPage);
    }

}
