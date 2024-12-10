using UnityEngine;

public class Fire : MonoBehaviour
{
    public float extinguishRate = 0.1f;
    private bool isExtinguished = false;
    private ParticleSystem[] childParticles;
    private AudioSource fireSound;


    private void Awake()
    {
        childParticles = GetComponentsInChildren<ParticleSystem>();
        fireSound = GetComponent<AudioSource>();
    }
    public void Extinguish()
    {
        if (isExtinguished || childParticles == null || childParticles.Length == 0) return;

        bool allParticlesShrunk = true;

        foreach (var particle in childParticles)
        {
            var mainModule = particle.main;
            mainModule.startSize = new ParticleSystem.MinMaxCurve(
                Mathf.Max(0, mainModule.startSize.constantMin - extinguishRate),
                Mathf.Max(0, mainModule.startSize.constantMax - extinguishRate)
            );

            var emission = particle.emission;
            emission.rateOverTime = Mathf.Max(0, emission.rateOverTime.constant - extinguishRate * 10);

            if (mainModule.startSize.constantMax > 0.1f || emission.rateOverTime.constant > 0)
            {
                allParticlesShrunk = false;
            }
        }

        if (allParticlesShrunk)
        {
            isExtinguished = true;

            fireSound.enabled = false;
            Destroy(gameObject);
        }
    }
}
