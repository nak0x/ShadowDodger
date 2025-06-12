using System.Collections;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        private bool _isGameEnded = false;

        [FormerlySerializedAs("checkPoints")] [Header("Level checkpoints")] [SerializeField]
        private LevelCheckPointsManager levelCheckPointsManager;

        [Header("Player")]
        [SerializeField] private PlayerManager player;

        public void EndLevel()
        {
            if (!_isGameEnded)
            {
                _isGameEnded = true;
                StartCoroutine(RestartLevel());
            }
        }

        public void GoToLastCheckpoint()
        {
            levelCheckPointsManager.SetCurrentToLastCheckPoint();
            player.TP(levelCheckPointsManager.currentCheckPointPosition);
        }

        private static IEnumerator RestartLevel()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}