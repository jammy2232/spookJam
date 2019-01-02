using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{

#pragma warning disable

    // When the game starts the game is on the main menu
    enum GameState { MainMenu, PauseMenu, Play, TutorialScreen, GameOver }
    GameState gameState = GameState.MainMenu;

    // This is a reference to the main menu/Tutorial Screen
    [Header("UI Elements")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject tutorialMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject mainGameUI;
    [SerializeField] GameObject gameOverUI;

    // Reference to the players and other objects to reset them and disable them
    List<GameObject> AllObjects = new List<GameObject>();

    [Header("GamePlay Settings")]
    [SerializeField] float initialSpawnTime;
    [SerializeField] float RateOfSpawnTimeIncrease;
    [SerializeField] float minSpawnTime;

    // Refernces to all the prefabs for controlling the game
    [Header("Game Objects and Start Positions")]
    [SerializeField] GameObject playerOnePrefab;
    [SerializeField] GameObject playerTwoPrefab;
    [SerializeField] Transform playerOneStartPos;
    [SerializeField] Transform playertwoStartPos;
    [SerializeField] GameObject enemyTypeOnePrefab;
    [SerializeField] GameObject enemyTypeTwoPrefab;
    [SerializeField] Transform enemyLeftSpawnerPos;
    [SerializeField] Transform enemyRightSpawnerPos;

    // References to ingame instances 
    List<GameObject> enemies = new List<GameObject>();
    GameObject playerOne;
    GameObject playerTwo;

    // Controlling variables
    float timer = 0.0f;
    float currentRate = 0.0f;

#pragma warning restore

    // set the initial case
    private void OnEnable()
    {

        // Force all the correct state of the menus
        mainMenu.SetActive(true);
        tutorialMenu.SetActive(false);
        gameOverUI.SetActive(false);
        mainGameUI.SetActive(false);
        pauseMenu.SetActive(false);

        // subscribe to relevent events 

    }

    // Use this for initialization
    void Start()
    {

        // Hide Cursor 
        Cursor.visible = false;

        // Set a target framerate for slower machines 
        Application.targetFrameRate = 30;

    }

    // Update is called once per frame
    void Update()
    {

        switch (gameState)
        {
            case GameState.Play:

                // Spawn an enemy at time intervals
                timer += Time.deltaTime;

                if (timer > currentRate)
                {
                    SpawnARandomEnemy();
                    timer = 0.0f;
                }

                // Check for both players are dead or one quits
                if ((playerOne.GetComponent<PlayerController>().playerState == PlayerController.PLAYERSTATE.dead
                    && playerTwo.GetComponent<PlayerController>().playerState == PlayerController.PLAYERSTATE.dead))
                {
                    gameOverUI.SetActive(true);
                    mainGameUI.SetActive(false);
                    gameState = GameState.GameOver;
                }

                if (Input.GetKeyDown(KeyCode.Escape) ||
                    Input.GetButtonDown("Quit1") ||
                    Input.GetButtonDown("Quit2"))
                {
                    ReloadScene();
                }

                break;

                case GameState.MainMenu:

                // Check for both players are dead or one quits
                if (Input.GetKeyDown(KeyCode.Escape) ||
                    Input.GetButtonDown("Quit1") ||
                    Input.GetButtonDown("Quit2"))
                {
                    Application.Quit();
                }

                if(Input.GetButtonDown("Start") || Input.GetButtonDown("Submit"))
                {
                    MoveToTutorial();
                }

               break;

            case GameState.TutorialScreen:

                if (Input.GetButtonDown("Start") || Input.GetButtonDown("Submit"))
                {
                    EndTutorial();
                }

                if (Input.GetKeyDown(KeyCode.Escape) ||
                    Input.GetButtonDown("Quit1") ||
                    Input.GetButtonDown("Quit2"))
                {
                    ReloadScene();
                }

                break;

            case GameState.GameOver:

                if (Input.GetButtonDown("Start") || Input.GetButtonDown("Submit"))
                {
                    ReloadScene();
                }

                if (Input.GetKeyDown(KeyCode.Escape) ||
                    Input.GetButtonDown("Quit1") ||
                    Input.GetButtonDown("Quit2"))
                {
                    ReloadScene();
                }

                break;

        }
        
    }

    // Main Menu Button Pressed 
    public void MoveToTutorial()
    {

        // Hide/Show Menus
        mainMenu.SetActive(false);
        tutorialMenu.SetActive(true);

        // Move the game state
        gameState = GameState.TutorialScreen;

        // Spawn/Reset everything for the game
        ResetGame();
        SpawnThePlayers();

    }


    // Tutorial Button Pressed 
    public void EndTutorial()
    {

        // Hide the main Menu
        tutorialMenu.SetActive(false);
        mainGameUI.SetActive(true);

        // Move the game state
        gameState = GameState.Play;

        // Unfreeze everything to play the game

    }


    public void SpawnThePlayers()
    {

        // Spawn the players in thier starting positions
        playerOne = Instantiate(playerOnePrefab);
        playerTwo = Instantiate(playerTwoPrefab);

        // Move the players to the spawn position
        playerOne.transform.position = playerOneStartPos.position;
        playerTwo.transform.position = playertwoStartPos.position;

    }

    public void SpawnARandomEnemy()
    {

        Vector3 position;

        // Pick a random spawner
        if(Random.Range(0,2) == 1)
        {
            position = enemyLeftSpawnerPos.position;
        }
        else
        {
            position = enemyRightSpawnerPos.position;
        }

        // Spawn a random z between -15 and 15
        position.z = Random.Range(-10.0f, 10.0f);

        GameObject enemy;

        // Store the reference to the enemy
        if (Random.Range(0, 2) == 1)
        {
            enemy = Instantiate(enemyTypeOnePrefab);
            // Set the enemy to attack the correct player
            enemy.GetComponent<EnemyAI>().Target = playerOne.transform;
        }
        else
        {
            enemy = Instantiate(enemyTypeTwoPrefab);
            // Set the enemy to attack the correct player
            enemy.GetComponent<EnemyAI>().Target = playerTwo.transform;
        }

        // apply the spawn position 
        enemy.transform.position = position;

    }


    void ResetGame()
    {

        currentRate = initialSpawnTime;

    }


    public void ReloadScene()
    {

        // todo Remove this awful hack
        SceneManager.LoadScene(0);

    }


    // Fired when a enemy dies
    void IncreaseDifficulty()
    {

        // check the min is not breachec
        if(currentRate > minSpawnTime)
        {

        // reduce the spawn rate
            currentRate -= RateOfSpawnTimeIncrease;
       
        }

    }


}
