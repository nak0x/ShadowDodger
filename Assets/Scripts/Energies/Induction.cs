using UnityEngine;

namespace Energies
{
    /// <summary>
    /// Represents an induction energy source that can be used to power devices or systems.
    /// </summary>
    public class Induction : MonoBehaviour
    {
        [Header("Induction Settings")]
        [Tooltip("The amount of energy this induction source provides per second.")]
        public float EnergyPerSecond = 10f;

        public LayerMask playerLayer;

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Assuming the player has a BatteryManager component
                Player.BatteryManager batteryManager = other.GetComponent<Player.BatteryManager>();
                if (batteryManager != null)
                {
                    // Recharge the player's battery
                    batteryManager.Charge(Time.deltaTime, EnergyPerSecond);
                }
            }
        }
    }
}