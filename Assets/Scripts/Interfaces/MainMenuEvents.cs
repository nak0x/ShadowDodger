using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    [Header("Cursors Manager")]
    [SerializeField] private CursorManager _cursorManager;

    [Header("UI Elements")]
    [SerializeField] private UIDocument _document;

    [Header("UI Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonClickSound;

    private Button _playButton;
    private Button _quitButton;
    private List<Button> _buttons = new List<Button>();

    void Awake()
    {
        if (_document == null)
            Debug.LogError("UIDocument is not assigned in MainMenuEvents.");

        // Setup the UI elements
        _playButton = QueryDocument("PlayButton") as Button;
        _playButton.RegisterCallback<ClickEvent>(OnPlayGameCkicked);

        _quitButton = QueryDocument("QuitButton") as Button;
        _quitButton.RegisterCallback<ClickEvent>(OnQuitGameClicked);

        _buttons = _document.rootVisualElement.Query<Button>().ToList();
        foreach (var button in _buttons)
        {
            button.RegisterCallback<ClickEvent>(PlayButtonSound);
            button.RegisterCallback<MouseEnterEvent>(evt => _cursorManager.SetHoverCursor());
            button.RegisterCallback<MouseLeaveEvent>(evt => _cursorManager.SetNormalCursor());
        }
    }

    private void PlayButtonSound(ClickEvent evt)
    {
        _audioSource?.PlayOneShot(_buttonClickSound, .9f);
    }

    private void OnPlayGameCkicked(ClickEvent evt)
    {
        // Add logic to start the game here
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadGame();
        }
    }

    private void OnQuitGameClicked(ClickEvent evt)
    {
        Application.Quit();
    }

    void OnDisable()
    {
        _playButton?.UnregisterCallback<ClickEvent>(OnPlayGameCkicked);
        _quitButton?.UnregisterCallback<ClickEvent>(OnQuitGameClicked);

        foreach (var button in _buttons)
        {
            button?.UnregisterCallback<ClickEvent>(PlayButtonSound);
        }
    }

    private VisualElement QueryDocument(string name)
    {
        if (_document == null)
        {
            Debug.LogError("UIDocument is not assigned.");
            return null;
        }

        var root = _document.rootVisualElement;
        var element = root.Q<VisualElement>(name);
        if (element == null)
        {
            Debug.LogError($"Element with name '{name}' not found in the document.");
        }
        return element;
    }
}
