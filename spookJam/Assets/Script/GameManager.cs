using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

    // When the game starts the game is on the main menu
    enum GameState { MainMenu, PauseMenu, Play, TutorialScreen }
    GameState gameState = GameState.MainMenu;

    // This is a reference to the main menu/Tutorial Screen
    public GameObject mainMenu;
    public GameObject tutorialMenu;
    public GameObject pauseMenu;
    public GameObject mainGameUI;

    // Reference to the players and other objects to reset them and disable them
    List<GameObject> AllObjects = new List<GameObject>();

    // set the initial case
    private void OnEnable()
    {

        // Force all the correct state of the menus
        mainMenu.SetActive(true);
        tutorialMenu.SetActive(false);

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Main Menu Button Pressed 
    public void StartGame()
    {

        // Hide/Show Menus
        mainMenu.SetActive(false);
        tutorialMenu.SetActive(true);

        // Move the game state
        gameState = GameState.TutorialScreen;

        // Spawn/Reset everything for the game

    }


    // Tutorial Button Pressed 
    public void EndTutorial()
    {

        // Hide the main Menu
        tutorialMenu.SetActive(false);

        // Move the game state
        gameState = GameState.Play;

        // Unfreeze everything to play the game

    }






}
