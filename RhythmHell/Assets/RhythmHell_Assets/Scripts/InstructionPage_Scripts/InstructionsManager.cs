using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InstructionsManager : MonoBehaviour {

    [Header("Instructions Trackers")]
    public int pageIndex = 0;
    public string currentString;

    [Header("Instructions")]
    [TextArea] public string[] instructionTitles;
    [TextArea] public string[] instructions;

    [Header("GUI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI pageNumber;

    [Header("Scene Names")]
    public string mainMenuScene;

    public void Awake()
    {
        pageNumber.text = (pageIndex + 1) + "/" + instructions.Length.ToString();
    }

    public void NextPage()
    {
        pageIndex = Mathf.Clamp(pageIndex + 1, 0, instructions.Length - 1);
        currentString = instructions[pageIndex];

        titleText.text = instructionTitles[pageIndex];
        instructionText.text = currentString;
        pageNumber.text = (pageIndex + 1) + "/" + instructions.Length.ToString();
    }

    public void PreviousPage()
    {
        pageIndex = Mathf.Clamp(pageIndex - 1, 0, instructions.Length - 1);
        currentString = instructions[pageIndex];

        titleText.text = instructionTitles[pageIndex];
        instructionText.text = currentString;
        pageNumber.text = (pageIndex + 1) + " / " + instructions.Length.ToString();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

}
