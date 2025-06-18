using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour, Utils.IDevSerializable
    {
        [Header("Player objects")]
        [SerializeField] private GameObject _playerBody;
        [SerializeField] private Camera _playerCamera;

        [Header("Player properties")]
        [SerializeField] private List<GameObject> _playerGOs;
        [SerializeField] private string _playerName = "Player";

        private List<IPlayerProperty> _playerProperties = new List<IPlayerProperty>();

        public void Awake()
        {
            foreach (var go in _playerGOs)
            {
                bool hasProperty = go.TryGetComponent<IPlayerProperty>(out var tempProperty);
                Debug.Log($"Initializing player property from GameObject: {go.name} | Has IPlayerProperty: {hasProperty}");
                if (go.TryGetComponent<IPlayerProperty>(out var property))
                {
                    Debug.Log($"Adding property: {property.GetType().Name} from GameObject: {go.name}");
                    _playerProperties.Add(property);
                }
                else
                {
                    Debug.LogWarning($"GameObject {go.name} does not implement IPlayerProperty interface.");
                }
            }
        }

        public void TP(Vector3 position)
        {
            if (_playerBody == null)
            {
                Debug.LogError("Player body is not assigned!");
                return;
            }
            _playerBody.transform.position = position;
        }

        public void ResetPlayer(PlayerResetType resetType = PlayerResetType.Medium)
        {
            foreach (var property in _playerProperties)
            {
                Debug.Log($"Resetting property: {property.GetType().Name} with reset type: {resetType}");
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
            return $"Player Name: {_playerName}, Body: {_playerBody.name}, Camera: {_playerCamera.name}, Properties: [{properties}]";
        }
    }

    public enum PlayerResetType
    {
        Light, // Resets properties without effect on the player state
        Medium, // Resets properties and player state (e.g, death, respawn)
        Heavy // Deap reset, resets everything including the player state and properties (e.g, level restart)
    }

    public interface IPlayerProperty
    {
        public void ResetProperty(PlayerResetType resetType = PlayerResetType.Medium);
    }
}
