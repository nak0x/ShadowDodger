using UnityEngine;

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

        public override void Enter()
        {
            /* Do nothing for now */
        }

        public override void Exit()
        {
            /* Exiting a nothing state does nothing */
        }
        
        public override void Update()
        {
            /* Updating */
        }
    }

    public class MoveState : PlayerState
    {
        public MoveState(PlayerManager manager) : base(manager) {}

        public override void Enter()
        {
            /* Do nothing for now */
        }

        public override void Exit()
        {
            /* Exiting a nothing state does nothing */
        }
        
        public override void Update()
        {
            /* Updating */
        }
    }

    public class DeadState : PlayerState
    {
        public DeadState(PlayerManager manager) : base(manager) {}

        public override void Enter()
        {
            /* Do nothing for now */
        }

        public override void Exit()
        {
            /* Exiting a nothing state does nothing */
        }
        
        public override void Update()
        {
            /* Updating */
        }
    }

    public class PlayerStateMachine : MonoBehaviour
    {
        private PlayerState _current;

        public void ChangeState(PlayerState next)
        {
            _current?.Exit();
            _current = next;
            _current.Enter();
        }
        
        void Update() => _current?.Update();
    }
}