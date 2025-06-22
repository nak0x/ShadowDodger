using Collectibles;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Collects items when the player collides with them.
    /// </summary>
    public class Collector : MonoBehaviour
    {
        public void Collect(CollectibleData collectible)
        {
            Debug.Log($"Collected: {collectible.collectibleName}");
        }
    }
}