using System.Collections.Generic;
using Collectibles;
using UnityEngine;
using Utils;
using Level;

namespace Player
{
    public class PlayerManager : MonoBehaviour, IDevSerializable
    {
        [Header("Game Manager")]
        [SerializeField] private LevelManager _levelManager;

        [Header("Player objects")]
        [SerializeField] private GameObject _playerBody;
        [SerializeField] private Camera _playerCamera;

        [Header("Player properties")]
        [SerializeField] private new string name = "Player";
        [SerializeField] private CheckPointManager _checkPointManager;
        [SerializeField] List<ResetableMonoBehaviour> _playerProperties = new List<ResetableMonoBehaviour>();

        private List<CollectibleData> _scrapsCollected;

        public void TP(Vector3 position)
        {
            if (_playerBody == null)
            {
                Debug.LogError("Player body is not set for PlayerManager!");
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

        public void RegisterDeath(Death cause)
        {
            ResetPlayer(PlayerResetType.Medium);
            // Handle player death logic here, e.g., notify UI, play sound, etc.
            switch (cause)
            {
                case Death.Energy:
                    if (_checkPointManager == null)
                    {
                        Debug.LogError("CheckPointManager is not set for PlayerManager!");
                        return;
                    }
                    _checkPointManager.GoToCheckPoint();
                    break;
                case Death.GameOver:
                    if (_levelManager == null)
                    {
                        Debug.LogError("LevelManager is not set for PlayerManager!");
                        return;
                    }
                    _levelManager.EndLevel();
                    break;
                case Death.Damage:
                    Debug.Log("Player died due to enemy attack.");
                    break;
                default:
                    Debug.Log("Player died due to unknown reason.");
                    break;
            }
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
            return $"Player Name: {name}, Body: {_playerBody.name}, Camera: {_playerCamera.name}, Properties: [{properties}], Collected Scraps: {_scrapsCollected?.Count ?? 0}";
        }
    }

    public enum PlayerResetType
    {
        Light, // Resets properties without effect on the player state
        Medium, // Resets properties and player state (e.g, death, respawn)
        Heavy // Deap reset, resets everything including the player state and properties (e.g, level restart)
    }
}
