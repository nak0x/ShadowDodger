using UnityEngine;

namespace Player
{
    public class BatteryManager : MonoBehaviour
    {
        [Header("Game Manager")]
        [SerializeField] private PlayerLifeManager lifeManager;

        [Header("Energy Stats")]
        [Range(0, 100)] public float batteryPercentage = 100f;
        public float chargeRate = 5f;
        public float drainRate = 10f;
        
        [Header("Dev mod")]
        public bool debug = false;

        private bool _inCharge = false;

        public void Charge(float deltaTime, float chargeSpeed = 1) => batteryPercentage = Mathf.Min(100, batteryPercentage + chargeRate * chargeSpeed * deltaTime);
        public void Drain(float deltaTime) => batteryPercentage = Mathf.Max(0, batteryPercentage - drainRate * deltaTime);
        
        public bool GetInCharge() => _inCharge;
        public void SetInCharge(bool value) => _inCharge = value;

        private void Update()
        {
            if (debug)
                Debug.Log("Battery Level : " + batteryPercentage + " Charge state : " + _inCharge);
        }
    }
}