using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        private bool _isGameEnded = false;

        [Header("Player")]
        [SerializeField] private PlayerManager player;

        public void EndLevel()
        {
            if (!_isGameEnded)
            {
                _isGameEnded = true;
                RestartLevel();
            }
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}