using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public abstract class GameState
    {
        protected GameManager _gameManager;

        public GameState()
        {
            _gameManager = GameManager.Instance;
        }

        public abstract void HandleInit();

        public abstract void HandleMenu();

        public abstract void HandlePlaying();
    }

    class ConcreteGameStateInit : GameState
    {
        public override void HandleInit()
        {
            Debug.LogWarning("InitState is handling init mutate request");
        }

        public override void HandlePlaying()
        {
            Debug.LogError("Cannot imidiate play game right after init");
        }

        public override void HandleMenu()
        {
            _gameManager.TransitionTo(new ConcreteGameStateMenu());
            SceneManager.LoadSceneAsync((int)SceneIndexes.MENU, LoadSceneMode.Additive);
        }
    }

    class ConcreteGameStateMenu : GameState
    {
        public override void HandleMenu()
        {
            Debug.LogWarning("MenuState is handling menu mutate request");
        }

        public override void HandleInit()
        {
            Debug.LogWarning("MenuState is handling init mutate request");
        }

        public override void HandlePlaying()
        {
            _gameManager.TransitionTo(new ConcreteGameStatePlaying());

            _gameManager.AddToSceneLoadProgress(SceneManager.UnloadSceneAsync((int)SceneIndexes.MENU));
            _gameManager.AddToSceneLoadProgress(SceneManager.LoadSceneAsync((int)SceneIndexes.LEVEL, LoadSceneMode.Additive));
        }
    }

    class ConcreteGameStatePlaying : GameState
    {
        public override void HandlePlaying()
        {
            Debug.LogWarning("PlayingState is handling playing mutate request");
        }

        public override void HandleMenu()
        {
            _gameManager.TransitionTo(new ConcreteGameStateMenu());
            _gameManager.AddToSceneLoadProgress(SceneManager.UnloadSceneAsync((int)SceneIndexes.LEVEL));
            _gameManager.AddToSceneLoadProgress(SceneManager.LoadSceneAsync((int)SceneIndexes.MENU, LoadSceneMode.Additive));
        }

        public override void HandleInit()
        {
            Debug.LogWarning("PlayingState is handling init mutate request");
        }
    }
}