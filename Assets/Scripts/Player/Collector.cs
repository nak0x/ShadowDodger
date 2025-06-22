using Collectibles;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Collects items when the player collides with them.
    /// </summary>
    public class Collector : MonoBehaviour
    {

        [Header("Collector Properties")]
        [SerializeField] private PlayerManager playerManager;

        void Awake()
        {
            if (playerManager == null)
            {
                Debug.LogError("PlayerManager is not assigned in Collector!");
            }
        }

        public void Collect(CollectibleData collectible)
        {
            if (collectible.type == CollectibleType.Scraps)
            {
                playerManager.AddScrap(collectible);
            }
        }
    }
}