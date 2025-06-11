using Player;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Unity.VisualScripting;
using UnityEngine.Serialization;

[SerializeField]
internal class PlayerMouvement : MonoBehaviour
{
    
    // Camera rotation
    [Header("Camera rotation")]
    [SerializeField] private float mouseSensitivity = 2f;
    private float _verticalRotation = 0f;
    
    // Ground Movement
    [Header("Movement")]
    [SerializeField] private Rigidbody PlayerRB;
    [SerializeField] private BoxCollider PlayerCol;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Power Sources")]
    [SerializeField] private BatteryManager battery;

    private float _moveHorizontal;
    private float _moveForward;

    private void Start()
    {
        PlayerRB.freezeRotation = true;
    }

    private void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveForward = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        
        // Prevent diagonal movement: prioritize forward
        if (Mathf.Abs(_moveHorizontal) > 0 && Mathf.Abs(_moveForward) > 0)
        {
            _moveHorizontal = 0; // Remove this line to prioritize horizontal instead
        }

        Vector3 movement = (PlayerRB.transform.right * _moveHorizontal + PlayerRB.transform.forward * _moveForward).normalized;
        Vector3 targetVelocity = movement * moveSpeed;

        // Apply movement to the Rigidbody
        Vector3 velocity = PlayerRB.linearVelocity;

        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        
        PlayerRB.linearVelocity = velocity;

        // If we aren't moving, stop velocity so we don't slide
        if (_moveHorizontal == 0 && _moveForward == 0)
        {
            PlayerRB.linearVelocity = new Vector3(0, PlayerRB.linearVelocity.y, 0);
        }
    }
}
