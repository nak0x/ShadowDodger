using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour, Utils.IDevSerializable
    {
        [Header("Player objects")]
        [SerializeField] private GameObject _playerBody;
        [SerializeField] private Camera _playerCamera;

        [SerializeField] private List<IPlayerProperty> _playerProperties;
        [SerializeField] private string _playerName = "Player";

        public void TP(Vector3 position)
        {
            _playerBody.transform.position = position;
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
