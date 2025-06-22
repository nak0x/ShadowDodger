using UnityEngine;

namespace Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Detection settings")]
        [SerializeField] private LayerMask playerLayer;

        [Header("Checkpoint visuals")]
        [SerializeField] private Renderer checkpointRenderer;
        [SerializeField] private string colorField = "_EmissionColor";
        [SerializeField] private Color triggeredColor = Color.white;

        private bool _triggered = false;

        public void OnTriggerEnter(Collider other)
        {
            if (!_triggered && ((1 << other.gameObject.layer) & playerLayer.value) != 0)
            {
                TriggerCheckpoint(other);
            }
        }

        private void TriggerCheckpoint(Collider other)
        {
            // Assuming the player has a PlayerManager component to handle checkpoint logic
            var playerManager = other.GetComponent<Player.CheckPointManager>();
            if (playerManager != null)
            {
                playerManager.SetCheckPoint(transform.position);
            }
            else
            {
                Debug.LogWarning("PlayerManager not found on the player object.");
            }

            // Change visuals to indicate the checkpoint has been triggered
            UpdateCheckpointVisuals();
        }

        private void UpdateCheckpointVisuals()
        {
            checkpointRenderer.material.SetColor(colorField, triggeredColor);
            _triggered = true;
        }
    }
}
