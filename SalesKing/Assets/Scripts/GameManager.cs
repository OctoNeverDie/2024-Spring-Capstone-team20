using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        CurrentGameState = GameState.Playing;
        // Add more logic for starting the game
    }

    public void PauseGame()
    {
        CurrentGameState = GameState.Paused;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        CurrentGameState = GameState.Playing;
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        CurrentGameState = GameState.GameOver;
        // Add more logic for ending the game
    }
}

