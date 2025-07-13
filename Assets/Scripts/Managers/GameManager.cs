using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject LoadingScreen;

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    private GameState _state;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Enforce singleton
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes

        InitializeGame();
    }

    void InitializeGame()
    {
        // Transition to a new GameState.
        TransitionTo(new ConcreteGameStateInit());
        DisplayMenu();
    }

    public IEnumerator GetsSceneLoadProgress()
    {
        foreach (AsyncOperation sceneLoading in scenesLoading)
        {
            while (!sceneLoading.isDone)
            {
                yield return null;
            }
        }

        LoadingScreen.SetActive(false);
        DisplayMenu();
    }

    public void AddToSceneLoadProgress(AsyncOperation operation)
    {
        scenesLoading.Add(operation);
    }

    // ==== Game State Managment ====

    public void TransitionTo(GameState state)
    {
        _state = state;
    }

    public void PauseGame()
    {
        throw new NotImplementedException();
    }

    public void StartGame()
    {
        LoadingScreen.SetActive(true);
        _state.HandlePlaying();
        StartCoroutine(GetsSceneLoadProgress());
    }

    public void DisplayMenu()
    {
        _state.HandleMenu();
    }
}
