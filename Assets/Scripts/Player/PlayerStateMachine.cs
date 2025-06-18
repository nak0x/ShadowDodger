using UnityEngine;
using Events;

namespace Player
{
    public abstract class PlayerState
    {
        protected PlayerManager Manager;
        protected PlayerState(PlayerManager manager) {this.Manager = manager;}

        public virtual void Enter() {}
        public virtual void Exit() {}
        public virtual void Update() {}
    }

    public class IdleState : PlayerState
    {
        public IdleState(PlayerManager manager) : base(manager) {}

        public override void Enter() {}
        public override void Exit() {}
        public override void Update() {}
    }

    public class MoveState : PlayerState
    {
        public MoveState(PlayerManager manager) : base(manager) {}

        public override void Enter() {}
        public override void Exit() {}
        public override void Update() {}
    }

    public class DeadState : PlayerState
    {
        public DeadState(PlayerManager manager) : base(manager) {}

        public override void Enter() {}
        public override void Exit() {}
        public override void Update() {}
    }

    public class PlayerStateMachine : MonoBehaviour, Utils.IDevSerializable, IPlayerProperty
    {
        private PlayerState _current;

        [Header("Player Manager")]
        [SerializeField] private PlayerManager playerManager;

        private void OnEnable()
        {
            GameEvents.PlayerDied += OnPlayerDied;
            GameEvents.PlayerRespawned += OnPlayerRespawned;
        }

        private void OnDisable()
        {
            GameEvents.PlayerDied -= OnPlayerDied;
            GameEvents.PlayerRespawned -= OnPlayerRespawned;
        }

        private void Start()
        {
            // Initialize the player state machine with the idle state
            ChangeState(new IdleState(playerManager));
        }

        private void OnPlayerDied()
        {
            ChangeState(new DeadState(playerManager));
        }

        private void OnPlayerRespawned()
        {
            ChangeState(new IdleState(playerManager));
        }

        public void ChangeState(PlayerState next)
        {
            _current?.Exit();
            _current = next;
            _current.Enter();
        }

        void Update() => _current?.Update();

        public string GetStateName()
        {
            return _current?.GetType().Name ?? "None";
        }

        public string DevSerialize()
        {
            // Serialize the current state name
            return $"Player state : {GetStateName()}";
        }

        public void ResetProperty(PlayerResetType resetType = PlayerResetType.Medium)
        {
            // Reset the player state to idle on reset
            if (resetType != PlayerResetType.Light)
                ChangeState(new IdleState(playerManager));
        }
    }
}