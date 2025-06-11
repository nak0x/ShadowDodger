using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Player objects")]
        [SerializeField] private GameObject _playerBody;

        [SerializeField] private Camera _playerCamera;

        public void TP(Vector3 position)
        {
            Debug.Log("TP player to position : " + position);
            _playerBody.transform.position = position;
        }
    }
}
