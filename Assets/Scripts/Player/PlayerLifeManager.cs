using Unity.VisualScripting;
using UnityEngine;
using Level;

namespace Player
{
    public enum Death
    {
        Game = -1,
        Energy = 0,
        Damage = 1, 
    }

    public class PlayerLifeManager : MonoBehaviour
    {

        [Header("Life Settings")]
        public int maxLifes = 3;
        public int remainingLife;

        [Header("Player Energy")]
        [SerializeField] private BatteryManager battery;
    
        [Header("Game Integration")]
        [SerializeField] private LevelManager levelManager;

        private int _currentLife;

        public int GetCurrentLife()
        {
            return _currentLife;
        }

        void Start()
        {
            remainingLife = maxLifes;
            if (levelManager == null)
                Debug.LogError("No levelManager assigned to player life manager");
        }

        public void Die(Player.Death cause)
        {
            Debug.Log("Player is dead : " + cause);
            remainingLife--;
            if (remainingLife <= 0)
                levelManager.GoToLastCheckpoints();
            Resucite();
        }

        public void Resucite()
        {
            battery.refill();
        }

        public void Heal(int amount = 1)
        {
            remainingLife += amount;
            if (remainingLife > maxLifes)
                remainingLife = maxLifes;
        }
    }
}
