using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmKey : MonoBehaviour {

    public enum MoveAxis { X, Y };

    [Header("key Variables")]
    public Vector3 startPoint;
    public Vector3 destroyPoint;
    public Vector3 moveSpeed;
    public MoveAxis mAxis;
    public string timerTag;

    [Header("Key Sprite Array")]
    public Sprite[] heads;

    private void Start()
    {
        transform.position = startPoint;
        GetComponent<SpriteRenderer>().sprite = heads[Random.Range(0, heads.Length - 1)];
    }

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameManager>().gamePaused)
            transform.position += moveSpeed * Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.tag == timerTag)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
