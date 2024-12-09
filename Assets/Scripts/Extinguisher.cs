using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    LayerMask fireLayer;
    private bool isActive = false;
    private ParticleSystem extinguisherParticles;

    private void Awake()
    {
        fireLayer = LayerMask.GetMask("Fire");
        extinguisherParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
        extinguisherParticles.Stop();
    }

    private void Update()
    {
        if (isActive)
        {
            UseExtinguisher();
        }
        
    }

    public void StartExtinguish()
    {
        extinguisherParticles.Play();
        isActive = true;
    }

    public void StopExtinguishing()
    {
        extinguisherParticles.Stop();
        isActive = false;
    }

    public void UseExtinguisher()
    {
        Transform playerTransform = Camera.main.transform;
        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out RaycastHit hit, 3f, fireLayer))
        {
            Fire fire = hit.collider.GetComponent<Fire>();
            if (fire != null)
            {
                fire.Extinguish();
            }
        }
    }

    public bool CanExtinguish() {
        
        Transform playerTransform = Camera.main.transform;
        return Physics.Raycast(playerTransform.position, playerTransform.forward, 4f, fireLayer);
        
    }
}
