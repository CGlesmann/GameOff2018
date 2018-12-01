using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [Header("Manager References")]
    public RhythmGameManager rgManager;
    public EnemySpawner eSpawner;
    public RhythmSpawner rSpawner;

    [Header("Global Game Variables")]
    public bool gamePaused = false;
    public float gameStartBuffer = 60f;
    public int money;
    public string levelSelectionScene;
    public GameObject sfxPlayer;

    private float startTimer = 0f;

    [Header("Building Variables")]
    public NewTowerPlacer towerPlacer = null;
    [HideInInspector] public GameObject buildTower = null;

    [Header("GUI References")]
    public GameObject startTimerPanel;
    public GameObject pauseMenu;
    public GameObject victoryMenu;
    public GameObject gameOverMenu;
    public GameObject towerShopMenu;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI moneyText_O;
    public TextMeshProUGUI startTimerText;

    [Header("Shop Variables")]
    public RectTransform shopButton;
    public Image shopArrow;
    public Vector3 shopButtonClosed;
    public Vector3 shopButtonOpen;

    [Header("Background References")]
    public float point = 11f;
    public float scrollSpeed = 1f;
    public GameObject scrollingBack_1;
    public GameObject scrollingBack_2;

    [Header("Game Over Variables")]
    public bool gameOver = false;
    public int playerLives = 1;
    public AudioClip gameOverSound = null;

    [Header("Victory Variables")]
    public bool victory = false;
    public AudioClip victorySound = null;

    private void Awake()
    {
        startTimer = gameStartBuffer;
        buildTower = null;

        gamePaused = false;

        startTimerPanel.SetActive(true);
        gameOverMenu.SetActive(false);
    }

    private void Update()
    {
        if (!gamePaused)
        {
            // Updating the GUI
            UpdateGUI();

            // Updating the Background(s)
            UpdateScrollingBackground();

            // Counting down to start of level
            if (startTimer > 0f)
            {
                startTimer -= Time.deltaTime;
                if (startTimer <= 0f)
                {
                    StartLevel();
                    StartCoroutine("WaitForVictory");
                    startTimerPanel.SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }

        // Checking for a Game Over
        CheckForGameOver();
       
    }

    IEnumerator WaitForVictory()
    {
        float time = GetComponent<AudioSource>().clip.length;
        Debug.Log(GetComponent<AudioSource>().clip.name);
        Debug.Log("Time: " + time.ToString());

        yield return new WaitForSecondsRealtime(time);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        while (enemies.Length > 0)
            yield return new WaitForSeconds(0.1f);

        SetVictory();
    }

    public void ToggleTowerShop()
    {
        if (towerShopMenu != null)
        {
            towerShopMenu.SetActive(!towerShopMenu.activeSelf);
            shopArrow.transform.localScale = new Vector3(shopArrow.transform.localScale.x, shopArrow.transform.localScale.y * -1, shopArrow.transform.localScale.z);

            /*
            if (towerShopMenu.activeSelf)
            {
                Debug.Log(shopButtonOpen);
                shopButton.position = shopButtonOpen;
            }
            else
            {
                Debug.Log(shopButtonClosed);
                shopButton.position = shopButtonClosed;
            }
            */
        }
    }

    public void PlaySoundFX(AudioClip aClip)
    {
        SoundPlayer newPlayer = Instantiate(sfxPlayer).GetComponent<SoundPlayer>();
        newPlayer.playClip = aClip;
        newPlayer.SetUp();

        return;
    }

    public void SetVictory()
    {
        Debug.Log(" You Won! ");
        victory = true;
        PauseGame();

        pauseMenu.SetActive(false);
        victoryMenu.SetActive(true);

        if (victorySound != null)
        {
            GetComponent<AudioSource>().clip = victorySound;
            GetComponent<AudioSource>().Play();
        }
    }

    public void CheckForGameOver()
    {
        if (!gameOver && !gamePaused)
        {
            // Checking for lives
            if (playerLives <= 0)
            {
                gameOver = true;

                PauseGame();

                pauseMenu.SetActive(false);
                gameOverMenu.SetActive(true);

                // Play the sound effect
                if (gameOverSound != null)
                {
                    GetComponent<AudioSource>().clip = gameOverSound;
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    public void PauseGame()
    {
        // Activate the Menu
        pauseMenu.SetActive(true);

        // Pausing the Song
        rSpawner.PauseGame();

        gamePaused = true;
    }

    public void UnPauseGame()
    {
        // De-Activate the Menu
        pauseMenu.SetActive(false);

        rSpawner.UnPauseGame();

        gamePaused = false;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitLevel()
    {
        // Going back to the Level Selection
        SceneManager.LoadScene(levelSelectionScene);
    }

    private void StartLevel()
    {
        // Starting Enemy Spawning and Rhythm Game
        eSpawner.StartGame();
        rSpawner.StartGame();
        rgManager.StartGame();
    }

    private void UpdateGUI()
    {
        // Updating Money Text
        moneyText.text = money.ToString("C");
        moneyText_O.text = money.ToString("C");

        //Updating Start Timer
        if (startTimer > 0f)
        {
            startTimerText.text = startTimer.ToString("F0");
        }
    }

    private void UpdateScrollingBackground()
    {
        if (scrollingBack_1 != null && scrollingBack_2 != null)
        {
            // Moving both of the backgrounds
            scrollingBack_1.transform.position = new Vector3(scrollingBack_1.transform.position.x, scrollingBack_1.transform.position.y + (scrollSpeed * Time.deltaTime), scrollingBack_1.transform.position.z);
            scrollingBack_2.transform.position = new Vector3(scrollingBack_2.transform.position.x, scrollingBack_2.transform.position.y + (scrollSpeed * Time.deltaTime), scrollingBack_2.transform.position.z);

            // Checking for out of bounds
            if (scrollingBack_1.transform.position.y >= 11f)
            {
                scrollingBack_1.transform.position = new Vector3(scrollingBack_1.transform.position.x, -point, scrollingBack_1.transform.position.z);
            }

            if (scrollingBack_2.transform.position.y >= 11f)
            {
                scrollingBack_2.transform.position = new Vector3(scrollingBack_2.transform.position.x, -point, scrollingBack_2.transform.position.z);
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
