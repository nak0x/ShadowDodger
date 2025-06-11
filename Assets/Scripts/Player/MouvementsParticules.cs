using System;
using UnityEditor.Timeline;
using UnityEngine;

public class MouvementsParticules : MonoBehaviour
{

    [Header("Particle System")]
    [SerializeField] private ParticleSystem particles;
    [SerializeField] Transform particlesTransform;
    [SerializeField] private Rigidbody PlayerRB;

    [Header("Settings")]
    [SerializeField] private float progressionDuration = 2f;
    [SerializeField] private float rateFactor = 20f;

    private float _mouvementStartTime = 0f; 

    void Update()
    {
        var magnitude = PlayerRB.linearVelocity.magnitude;

        if (magnitude > 0 && _mouvementStartTime == 0f)
        {
            _mouvementStartTime = Time.time;
        } else if (magnitude == 0 && _mouvementStartTime != 0f)
        {
            _mouvementStartTime = 0f;
        }
        
        var emission = particles.emission;
        emission.rateOverDistance = rateFactor * Mathf.Lerp(0f, magnitude, (Time.time - _mouvementStartTime)/progressionDuration);
        
        particlesTransform.rotation = Quaternion.LookRotation(-PlayerRB.linearVelocity);
    }
}
