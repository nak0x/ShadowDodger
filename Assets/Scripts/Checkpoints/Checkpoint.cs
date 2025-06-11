using UnityEngine;

namespace Checkpoints
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private Level.LevelCheckPointsManager manager;

        [Header("Detection settings")]
        [SerializeField] private LayerMask playerLayer;

        public void OnTriggerEnter(Collider collider)
        {
            Debug.Log(collider.gameObject.name);
            if (collider.gameObject.layer == playerLayer.value)
            {
                manager.SetCurrentCheckPoint(gameObject);
            }
        }
    }
}
