using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DemonologyPageManager : MonoBehaviour {

    [Header("Global GUI Variables")]
    public TowerStats buttonReference = null;

    [Header("Global GUI References")]
    public GameObject statPanelParent;
    public TextMeshProUGUI towerDescription;

    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI towerTagText;

    public TextMeshProUGUI towerHealthText;
    public TextMeshProUGUI towerDamageText;
    public TextMeshProUGUI towerSpeedText;

    [Header("Level Selection Screen Ref")]
    public string levelSelectScreen;

    private void Awake()
    {
        DeactivatePage();
    }

    public void ActivatePage()
    {
        statPanelParent.SetActive(true);
        towerDescription.text = buttonReference.towerDescription;

        TowerStats.TowerTags currentTag;
        string tags = "";
        for (int i = 0; i < buttonReference.tags.Length; i++)
        {
            currentTag = buttonReference.tags[i];
            if (currentTag == TowerStats.TowerTags.Bones)
                tags += "Bone";
            if (currentTag == TowerStats.TowerTags.Mortal)
                tags += "Mortal";
            if (currentTag == TowerStats.TowerTags.Demon)
                tags += "Demon";

            if ((i + 1) < buttonReference.tags.Length)
                tags += ", ";
        }

        towerNameText.text = buttonReference.towerName;
        towerTagText.text = tags;

        towerHealthText.text = buttonReference.towerHealth.ToString();
        towerDamageText.text = buttonReference.towerDamage.ToString();
        towerSpeedText.text = buttonReference.moveVector.x.ToString() + "u";

    }

    public void DeactivatePage()
    {
        statPanelParent.SetActive(false);
    }

    public void GoBackToLevelSelect()
    {
        SceneManager.LoadScene(levelSelectScreen);
    }

}
