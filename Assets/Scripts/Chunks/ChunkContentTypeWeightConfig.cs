using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Chunks
{
    [CreateAssetMenu(fileName = "ChunkContentTypeWeightConfig", menuName = "Game/Chunk Content Type Config")]
    public class ChunkContentTypeConfig : ScriptableObject
    {
        /// <summary>
        /// Specifies the different types of content that a chunk can contain.
        /// </summary>
        public enum ChunkContentType
        {
            CheckPoint = 0,
            EnergieSource = 1,
            Loot = 2,
            Empty = 3
        }

        
        /// <summary>
        /// Represents a weighted entry for a chunk content type.
        /// Each entry has a content type and a weight that determines its likelihood of being selected.
        /// </summary>
        [Serializable] public struct WeightedChunkContentType
        {
            public ChunkContentType Value;
            [Range(0, 100)] public float Weight;
        }

        /// <summary>
        /// Represents a content type with its minimum random rate.
        /// This allows for specifying a minimum rate for each content type to be selected,
        /// ensuring that certain types appear at least a specified rate.
        /// </summary>
        [Serializable] public struct ContentTypeMinRandomRate
        {
            public ChunkContentType Value;
            [Range(0, 100)] public float MinRandomRate;
        }

        [Header("Weight Distribution")]
        [Tooltip("List of weighted entries for chunk content types. Each entry has a content type and a weight that determines its likelihood of being selected.")]
        [SerializeField]
        public List<WeightedChunkContentType> Entries = new();

        [Header("Predefined Values (shared across all calls)")]
        [Tooltip("List of predefined chunk content types that can be reused across multiple calls to GetRandomValue(). This allows for consistent content generation without recalculating weights.")]
        /// <summary>
        /// List of predefined chunk content types that can be reused across multiple calls to GetRandomValue().
        /// This allows for consistent content generation without recalculating weights.
        /// </summary>
        public List<ChunkContentType> PredefinedValues = new();

        [Header("Content Type Minimum Random Rates")]
        [Tooltip("List of content types with their minimum random rates. This allows for specifying a minimum rate for each content type to be selected, ensuring that certain types appear at least a specified rate.")]
        public List<ContentTypeMinRandomRate> ContentTypeMinRandomRates = new();

        // Shared state per config asset
        private static readonly Dictionary<ChunkContentTypeConfig, Queue<ChunkContentType>> SharedPredefinedQueues = new();

        private static int GlobalRandomCallCount = 0;

        /// <summary>
        /// Retrieves a shared queue of predefined chunk content types.
        /// This queue is shared across all instances of this config asset, allowing for consistent content generation
        /// without recalculating weights each time.
        /// </summary>
        /// <returns>The global queues of ChunkContentType</returns>
        private Queue<ChunkContentType> GetSharedQueue()
        {
            if (!SharedPredefinedQueues.ContainsKey(this) || SharedPredefinedQueues[this] == null)
            {
                SharedPredefinedQueues[this] = new Queue<ChunkContentType>(PredefinedValues);
            }

            return SharedPredefinedQueues[this];
        }

        /// <summary>
        /// Resets the shared queue of predefined chunk content types.
        /// This method clears the current queue and repopulates it with the predefined values from this config asset.
        /// It is useful when you want to reuse the predefined values without recalculating weights.
        /// </summary>
        public void ResetSharedQueue()
        {
            SharedPredefinedQueues[this] = new Queue<ChunkContentType>(PredefinedValues);
        }

        /// <summary>
        /// Resets the global random call count.
        /// This method is useful for resetting the count when you want to start fresh with random value generation.
        /// It can be called at the beginning of a new game session or when you want
        /// </summary>
        public static void ResetGlobalRandomCallCount()
        {
            GlobalRandomCallCount = 0;
        }

        /// <summary>
        /// Gets a random chunk content type based on the weights defined in the Entries list.
        /// If the shared queue has predefined values, it will dequeue and return a value from there.
        /// If the queue is empty, it will calculate a random value based on the weights of the entries.
        /// </summary>
        /// <returns>A randomly selected ChunkContentType based on the defined weights.</returns>
        public ChunkContentType GetRandomValue()
        {
            GlobalRandomCallCount++;

            var queue = GetSharedQueue();
            if (queue.Count > 0)
                return queue.Dequeue();

            foreach (var entry in ContentTypeMinRandomRates)
            {
                if (GlobalRandomCallCount % entry.MinRandomRate == 0)
                {
                    return entry.Value;
                }
            }

            float total = 0f;
            foreach (var entry in Entries)
                total += entry.Weight;

            float rand = Random.Range(0, total);
            float cumulative = 0f;

            foreach (var entry in Entries)
            {
                cumulative += entry.Weight;
                if (rand <= cumulative)
                    return entry.Value;
            }

            return Entries.Count > 0 ? Entries[^1].Value : default;
        }
    }
}