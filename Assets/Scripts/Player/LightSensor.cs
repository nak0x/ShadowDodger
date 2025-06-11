using UnityEngine;

namespace Player
{
    public class LightSensor : MonoBehaviour
    {
        public float lightDetectionRadius = 2f;
        public LayerMask lightLayerMask;
        
        [SerializeField] BatteryManager battery;
        private Collider[] lightHits = new Collider[10];

        void Update()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, lightDetectionRadius, lightHits, lightLayerMask);
            float totalIntensity = 0f;

            for (int i = 0; i < count; i++)
            {
                var light = lightHits[i].GetComponent<Light>();
                if (light && light.enabled)
                {
                    Vector3 toLight = (light.transform.position - transform.position).normalized;
                    if (!Physics.Raycast(transform.position, toLight, out var hit, lightDetectionRadius))
                        totalIntensity += light.intensity; // optionally scale by distance, angle, etc.
                }
            }

            if (totalIntensity > 0f)
            {
                battery.SetInCharge(true);
                battery.Charge(Time.deltaTime);
            }
            else
            {
                battery.SetInCharge(false);
            }
        }
    }
}
