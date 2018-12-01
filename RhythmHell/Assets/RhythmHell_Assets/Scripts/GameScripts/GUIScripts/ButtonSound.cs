using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider2D))]
public class ButtonSound : MonoBehaviour {

    [Header("Sound refs")]
    public AudioSource aSource;
    public AudioClip clickSound;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicking");

        aSource.clip = clickSound;
        aSource.Play();
    }

}
