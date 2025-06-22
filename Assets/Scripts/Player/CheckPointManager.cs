using UnityEngine;

namespace Player
{
    /// <summary>
    /// Manages the player's checkpoints.
    /// </summary>
    public class CheckPointManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;

        private Vector3 _currentCheckPoint;

        public void GoToCheckPoint()
        {
            if (_currentCheckPoint == Vector3.zero)
            {
                Debug.LogWarning("No checkpoint set. Cannot teleport to checkpoint.");
                return;
            }

            playerManager.TP(_currentCheckPoint);
        }

        public void SetCheckPoint(Vector3 position)
        {
            _currentCheckPoint = position;
        }
    }
}