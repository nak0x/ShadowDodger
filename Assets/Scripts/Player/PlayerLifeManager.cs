using Unity.VisualScripting;
using UnityEngine;
using Level;
using Utils;

namespace Player
{
    public enum Death
    {
        GameOver = -1,
        Energy = 0,
        Damage = 1, 
    }

    public class PlayerLifeManager : ResetableMonoBehaviour, Utils.IDevSerializable
    {

        [Header("Life Settings")]
        public int maxLifes = 3;
        public int remainingLife;

        [Header("Game Integration")]
        [SerializeField] private PlayerStateMachine playerState;
        [SerializeField] private PlayerManager playerManager;


        private int _currentLife;

        public int GetCurrentLife()
        {
            return _currentLife;
        }

        void Start()
        {
            remainingLife = maxLifes;
        }

        public void Die(Death cause)
        {
            playerState.ChangeState(new DeadState(playerManager));
            remainingLife--;
            if (remainingLife <= 0)
            {
                playerManager.RegisterDeath(Death.GameOver);
                return;
            }
            playerManager.RegisterDeath(cause);
        }

        public void Heal(int amount = 1)
        {
            remainingLife += amount;
            if (remainingLife > maxLifes)
                remainingLife = maxLifes;
        }

        public string DevSerialize()
        {
            return $"Player remaining lifes : {remainingLife}";
        }

        public override void ResetProperty(PlayerResetType resetType = PlayerResetType.Medium)
        {
            if (resetType == PlayerResetType.Heavy)
            {
                remainingLife = maxLifes;
                _currentLife = maxLifes;
            }
        }
    }
}
