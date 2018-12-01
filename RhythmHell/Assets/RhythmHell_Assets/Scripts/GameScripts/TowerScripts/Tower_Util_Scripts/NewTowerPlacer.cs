using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NewTowerPlacer : MonoBehaviour {

    [Header("Variables")]
    public LayerMask laneMask;
    public Sprite displaySprite;
    public Color spriteColor;

    public GameObject buildTower;
    public float yOffset;

    private SpriteRenderer sr;
    private int laneIndex = -1;

    [Header("GameObject Reference")]
    public GameObject[] lanes;

    public void SetUpPlacer(GameObject tower)
    {
        sr = GetComponent<SpriteRenderer>();

        displaySprite = tower.GetComponent<SpriteRenderer>().sprite;
        spriteColor = tower.GetComponent<SpriteRenderer>().color;
        spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, spriteColor.a / 2f);

        sr.sprite = displaySprite;
        sr.color = spriteColor;

        buildTower = tower;

        transform.localScale = buildTower.transform.localScale;
    }

    private void Update()
    {
        SetPosition();

        // Checking for Tower Placement Input
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 topLeft = new Vector2(lanes[0].GetComponent<SpriteRenderer>().bounds.min.x, lanes[0].GetComponent<SpriteRenderer>().bounds.max.y);
            Vector2 bottomRight = new Vector2(lanes[4].GetComponent<SpriteRenderer>().bounds.max.x, lanes[4].GetComponent<SpriteRenderer>().bounds.min.y);

            Debug.Log("UPos: " + Input.mousePosition);
            Debug.Log("Pos: " + transform.position);
            Debug.Log("TL: " + topLeft);
            Debug.Log("BR: " + bottomRight);
            

            //Camera.main.ScreenToViewportPoint(Input.mousePosition).y < yPos
            //mousePos.x > topLeft.x && mousePos.x < bottomRight.x && mousePos.y < topLeft.y && mousePos.y > bottomRight.y
            if (mousePos.x > topLeft.x && mousePos.x < bottomRight.x && mousePos.y < topLeft.y && mousePos.y > bottomRight.y)
            {
                // Place the Tower
                GameObject newTower = Instantiate(buildTower);
                newTower.transform.position = transform.position;
                newTower.GetComponent<Tower>().towerLane = laneIndex;

                // Taking Away The Money
                GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
                manager.money -= (int)buildTower.GetComponent<Tower>().tStats.towerCost;

                // Deactivating if not enough money
                if (manager.money < (int)buildTower.GetComponent<Tower>().tStats.towerCost)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            gameObject.SetActive(false);
        }
    }

    private void SetPosition()
    {
        // Setting Position to Mouse Position
        Vector3 mousePos = Input.mousePosition;
        Vector3 translatedMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 newPos = new Vector3(translatedMousePos.x, translatedMousePos.y, 0f);

        // Snapping It to the nearest lane Position
        float xPos = 0f;
        float yPos = 0f;
        float current_diff = -1f;
        float lowest_diff = Mathf.Infinity;

        for(int i = 0; i < lanes.Length; i++)
        {
            current_diff = Mathf.Abs(lanes[i].transform.position.y - newPos.y);
            if (current_diff < lowest_diff)
            {
                lowest_diff = current_diff;

                SpriteRenderer lSR = lanes[i].GetComponent<SpriteRenderer>();

                xPos = Mathf.Clamp(newPos.x, lSR.bounds.min.x + 0.15f, lSR.bounds.max.x - 0.15f);
                yPos = lanes[i].transform.position.y + yOffset;
                laneIndex = i;
            }
        }

        newPos = new Vector3(xPos, yPos, newPos.z);

        transform.position = newPos;

        return;
    }

}
