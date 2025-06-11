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
}

public class PlayerLifeManager : MonoBehaviour
{

    [Header("Life Settings")]
    public int maxLifes = 3;
    public int remainingLife;
    
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
    }

    public void Die(Player.Death cause)
    {
        remainingLife--;
        if (remainingLife <= 0)
            levelManager.GoToLastCheckpoints();
    }

    public void Heal(int amount = 1)
    {
        remainingLife += amount;
        if (remainingLife > maxLifes)
            remainingLife = maxLifes;
    }
}
