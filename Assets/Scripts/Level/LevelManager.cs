using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        private bool _isGameEnded = false;

        [Header("Level Settings")]
        [SerializeField] private int levelIndex = 0;
        [SerializeField] private MapManager mapManager;

        [FormerlySerializedAs("checkPoints")] [Header("Level checkpoints")] [SerializeField]
        private LevelCheckPointsManager levelCheckPointsManager;

        [Header("Player")]
        [SerializeField] private PlayerManager player;

        public void Awake()
        {
            mapManager.GenerateLevel(levelIndex);
        }

        public void EndLevel()
        {
            if (!_isGameEnded)
            {
                _isGameEnded = true;
                RestartLevel();
            }
        }

        public void GoToLastCheckpoint()
        {
            levelCheckPointsManager.SetCurrentToLastCheckPoint();
            player.TP(levelCheckPointsManager.currentCheckPointPosition);
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}