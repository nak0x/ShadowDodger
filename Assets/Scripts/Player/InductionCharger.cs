using UnityEngine;

namespace Player
{
    public class InductionCharger : MonoBehaviour
    {
        [Header("Battery")]
        [SerializeField] private BatteryManager battery;

        [Header("Induction Settings")]
        [Range(0f, 100f)]
        public float inductionPower;
        public LayerMask inductionLayerMask;

        public void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter");
            int layer = other.gameObject.layer;
            if (layer == inductionLayerMask.value)
                battery.SetInCharge(true);
        }

        public void OnTriggerStay(Collider other)
        {
            int layer = other.gameObject.layer;
            if (layer == inductionLayerMask.value)
                battery.Charge(Time.deltaTime, inductionPower);
        }

        public void OnTriggerExit(Collider other)
        {
            int layer = other.gameObject.layer;
            if (layer == inductionLayerMask.value)
                battery.SetInCharge(false);
        }
    }
}
