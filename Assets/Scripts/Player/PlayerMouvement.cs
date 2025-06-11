using Player;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

internal class PlayerMouvement : MonoBehaviour
{
    
    // Ground Movement
    [Header("Movement")]
    [SerializeField] private Rigidbody PlayerRB;
    [SerializeField] private BoxCollider PlayerCol;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform bodyContainer;

    [Header("Power Sources")]
    [SerializeField] private BatteryManager battery;

    private float _moveHorizontal;
    private float _moveForward;
    private float _facingAngle;

    private void Start()
    {
        PlayerRB.freezeRotation = true;
    }

    private void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveForward = Input.GetAxisRaw("Vertical");
        _facingAngle = GetFacingAngle(_moveHorizontal, _moveForward);
        
        // Rotate player immediately when input changes
        bodyContainer.rotation = Quaternion.Euler(new Vector3(0, _facingAngle, 0));
        
        if (IsMoving())
            battery.Drain(Time.deltaTime);
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
            _moveHorizontal = 0; 
        }
        
        Vector3 localMovement = Vector3.zero;

        if (_moveForward != 0 || _moveHorizontal != 0)
        {
            localMovement = bodyContainer.forward * moveSpeed;
        }

        Vector3 velocity = PlayerRB.linearVelocity;
        velocity.x = localMovement.x;
        velocity.z = localMovement.z;

        PlayerRB.linearVelocity = velocity;

        // If we aren't moving, stop velocity so we don't slide
        if (_moveHorizontal == 0 && _moveForward == 0)
        {
            PlayerRB.linearVelocity = new Vector3(0, PlayerRB.linearVelocity.y, 0);
        }
    }
    
    
    private float GetFacingAngle(float horizontal, float forward)
    {
        if (horizontal < 0)
            return -90f;
        else if (horizontal > 0)
            return 90f;
        else if (forward < 0)
            return 180f;
        else if (forward > 0)
            return 0f;
        return _facingAngle; // Current rotation when no input
    }

    public bool IsMoving()
    {
        if (_moveForward != 0 || _moveHorizontal != 0)
            return true;
        return false;
    }
}
