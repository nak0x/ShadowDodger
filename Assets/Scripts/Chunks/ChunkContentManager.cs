using UnityEngine;
using System;

namespace Chunks
{

    [Serializable]
    public struct ChunkContentPrefabEntry
    {
        public ChunkContentTypeConfig.ChunkContentType ContentType;
        public GameObject Prefab;
    }

    /// <summary>
    /// ChunkContentManager is a component that manages the content of a chunk in a grid-based system.
    /// It handles the spawning of various types of content such as checkpoints, energy sources, and loot items.
    /// The content is spawned based on predefined spawn rates, allowing for dynamic and varied chunk content
    /// </summary>
    public class ChunkContentManager : MonoBehaviour
    {
        [Header("Chunk Content Prefab Mapping")]
        [SerializeField] private ChunkContentPrefabEntry[] contentPrefabs;

        [SerializeField] private ChunkContentTypeConfig contentTypeConfig;

        private ChunkContentTypeConfig.ChunkContentType _currentContentType;
        private GameObject _currentContentObject;

        void Start()
        {
            _currentContentType = contentTypeConfig.GetRandomValue();

            if (contentTypeConfig == null)
            {
                Debug.LogError("ChunkContentTypeConfig is not assigned in ChunkContentManager.");
                return;
            }

            SpawnContent(_currentContentType);
        }

        public void SpawnContent(ChunkContentTypeConfig.ChunkContentType contentType)
        {
            foreach (var entry in contentPrefabs)
            {
                if (entry.ContentType == contentType && entry.Prefab != null)
                {
                    _currentContentObject = Instantiate(entry.Prefab, transform.position, Quaternion.identity, transform);
                    return;
                }
            }
            Debug.LogWarning($"No prefab found for content type: {contentType}");
        }

        public void DestroyContent()
        {
            Destroy(_currentContentObject);
        }

        public void RenewContent()
        {
            if (_currentContentObject != null)
            {
                Destroy(_currentContentObject);
            }

            _currentContentType = contentTypeConfig.GetRandomValue();
            SpawnContent(_currentContentType);
        }
    }
}