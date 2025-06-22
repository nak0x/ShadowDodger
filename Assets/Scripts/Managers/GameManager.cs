using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject LoadingScreen;

    private string saveFilePath;

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
        saveFilePath = Application.persistentDataPath + "/save.json";
        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadGame()
    {
        LoadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.LEVEL, LoadSceneMode.Additive));

        StartCoroutine(GetsSceneLoadProgress());
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
    }
}
