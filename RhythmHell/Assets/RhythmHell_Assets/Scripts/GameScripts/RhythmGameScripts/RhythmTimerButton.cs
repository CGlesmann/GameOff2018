using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmTimerButton : MonoBehaviour {

    [Header("Image Variables")]
    public Color downColor;
    public Color upColor;

    [Header("Button Variables")]
    public KeyCode pressKey;
    public string eventTag;
    public int moneyReward;

    public float acceptance_range;

    private Image buttonUI;

    private void Start()
    {
        buttonUI = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(pressKey))
        {
            ClickButton();

            buttonUI.color = downColor;
        }

        if (Input.GetKeyUp(pressKey))
        {
            buttonUI.color = upColor;
        }
    }

    private void ClickButton()
    {
        GameObject[] rKeys = GameObject.FindGameObjectsWithTag(eventTag);
        foreach(GameObject rKey in rKeys)
        {
            // Getting X Distance
            float x_dist = rKey.transform.position.x - transform.position.x;
            Debug.Log(rKey.name + x_dist.ToString("F2"));

            if (x_dist > 0f && x_dist <= acceptance_range)
            {
                RhythmGameManager manager = GameObject.Find("GameManager").GetComponent<RhythmGameManager>();
                manager.NoteHit();
                //manager.rgMult = Mathf.Clamp(manager.rgMult + 0.25f, manager.rgMultMin, manager.rgMultMax);
                GameObject.Find("GameManager").GetComponent<GameManager>().money += moneyReward;

                GameObject.Destroy(rKey);
            } 
        }
    }
}
