using System.Collections.Generic;
using Collectibles;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerManager : MonoBehaviour, IDevSerializable
    {
        [Header("Player objects")]
        [SerializeField] private GameObject _playerBody;
        [SerializeField] private Camera _playerCamera;

        [Header("Player properties")]
        [SerializeField] List<ResetableMonoBehaviour> _playerProperties = new List<ResetableMonoBehaviour>();
        [SerializeField] private string _playerName = "Player";

        private List<CollectibleData> _scrapsCollected;

        public void TP(Vector3 position)
        {
            if (_playerBody == null)
            {
                Debug.LogError("Player body is not assigned!");
                return;
            }
            _playerBody.transform.position = position;
        }

        public void AddScrap(CollectibleData scrap)
        {
            if (_scrapsCollected == null)
            {
                _scrapsCollected = new List<CollectibleData>();
            }
            _scrapsCollected.Add(scrap);
        }

        public void ResetPlayer(PlayerResetType resetType = PlayerResetType.Medium)
        {
            foreach (var property in _playerProperties)
            {
                property.ResetProperty(resetType);
            }
        }

        public GameObject GetPlayerBody()
        {
            return _playerBody;
        }

        public string DevSerialize()
        {
            string properties = string.Join(", ", _playerProperties.ConvertAll(p => p.GetType().Name));
            return $"Player Name: {_playerName}, Body: {_playerBody.name}, Camera: {_playerCamera.name}, Properties: [{properties}], Collected Scraps: {_scrapsCollected?.Count ?? 0}";
        }
    }

    public enum PlayerResetType
    {
        Light, // Resets properties without effect on the player state
        Medium, // Resets properties and player state (e.g, death, respawn)
        Heavy // Deap reset, resets everything including the player state and properties (e.g, level restart)
    }
}
