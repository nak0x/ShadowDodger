using UnityEngine;

public class MouvementsParticules : MonoBehaviour
{

    [Header("Particle System")]
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Rigidbody PlayerRB;

    void Update()
    {
        var emission = particles.emission;
        emission.rateOverTime = PlayerRB.linearVelocity.magnitude;
    }
}
