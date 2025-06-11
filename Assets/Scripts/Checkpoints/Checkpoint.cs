using UnityEngine;

namespace Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private Level.LevelCheckPointsManager manager;

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
                TriggerCheckpoint();
                manager.SetCurrentCheckPoint(gameObject);
            }
        }

        private void TriggerCheckpoint()
        {
            checkpointRenderer.material.SetColor(colorField, triggeredColor);
            _triggered = true;
        }
    }
}
