using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour {

    [Header("Position Vars")]
    public Vector3 openPos;
    public Vector3 closedPos;

    [Header("Sound Refs")]
    public AudioClip clickSound;

    private bool opened = false;

    public void Toggle()
    {
        if (opened)
        {
            opened = false;
            transform.localPosition = closedPos;
        } else
        {
            opened = true;
            transform.localPosition = openPos;
        }

        if (clickSound != null)
        {
            GetComponent<AudioSource>().clip = clickSound;
            GetComponent<AudioSource>().Play();
        }

        GameObject.Find("GameManager").GetComponent<GameManager>().ToggleTowerShop();
    }

}
