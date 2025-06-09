using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        private bool _isGameEnded = false;

        [Header("Level checkpoints")] [SerializeField]
        private CheckPoints checkPoints;

        [Header("Player")] [SerializeField] private GameObject player;

        public void EndGame()
        {
            if (!_isGameEnded)
            {
                _isGameEnded = true;
                StartCoroutine(RestartLevel());
            }
        }

        public void GoToLastCheckpoints()
        {
            checkPoints.SetCurrentCheckPoint(checkPoints.GetLastCheckPoint());
            player.transform.position = checkPoints.currentCheckPointPosition;
        }

        private static IEnumerator RestartLevel()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}