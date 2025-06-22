using UnityEngine;
using Player;

namespace Collectibles
{
    /// <summary
    /// Collectibles types
    /// Represents the different types of collectibles in the game.
    /// </summary>
    public enum CollectibleType
    {
        Buff,
        Debuff,
        Scraps
    }

    public class Collectible : MonoBehaviour
    {
        [Header("Collectible Properties")]
        public CollectibleData collectibleData;

        void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out Collector collector);
            if (collector)
            {
                Collect(collector);
            }
        }

        /// <summary>
        /// Collects the collectible item.
        /// This method can be called when the player collects the item.
        /// </summary>
        public void Collect(Collector collector)
        {
            collector.Collect(collectibleData);
            Destroy(gameObject); // Destroy the collectible after collection
        }
    }
}