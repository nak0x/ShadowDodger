using Unity.VisualScripting;
using UnityEngine;
using Level;
using Utils;

namespace Player
{
    public enum Death
    {
        Game = -1,
        Energy = 0,
        Damage = 1, 
    }

    public class PlayerLifeManager : ResetableMonoBehaviour, Utils.IDevSerializable
    {

        [Header("Life Settings")]
        public int maxLifes = 3;
        public int remainingLife;

        [Header("Game Integration")]
        [SerializeField] private LevelManager levelManager;
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
            if (levelManager == null)
                Debug.LogError("No levelManager assigned to player life Manager");
        }

        public void Die(Death cause)
        {
            remainingLife--;
            if (remainingLife <= 0)
            {
                levelManager.EndLevel();
                return;
            }
            playerState.ChangeState(new DeadState(playerManager));
            playerManager.ResetPlayer(PlayerResetType.Medium);
            levelManager.GoToLastCheckpoint();
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
