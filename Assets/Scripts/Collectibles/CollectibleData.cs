using UnityEngine;

namespace Collectibles
{
    /// <summary>
    /// Represents the data for a collectible item.
    /// </summary>
    [CreateAssetMenu(fileName = "CollectibleData", menuName = "Game/Collectible", order = 1)]
    public class CollectibleData : ScriptableObject
    {
        [Header("Collectible Properties")]
        public string collectibleName;
        public Sprite collectibleIcon;
        public int collectibleValue;
        public CollectibleType collectibleType;
    }
}