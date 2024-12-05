using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    LayerMask fireLayer;
    private bool isActive = false;

    private void Awake()
    {
        fireLayer = LayerMask.GetMask("Fire");
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
        isActive = true;
    }

    public void StopExtinguishing()
    {
        isActive = false;
    }

    public void UseExtinguisher()
    {
        Transform playerTransform = Camera.main.transform;
        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out RaycastHit hit, 4f, fireLayer))
        {
            Fire fire = hit.collider.GetComponent<Fire>();
            if (fire != null) {
                fire.Extinguish();
            }
        }
    }

    public bool CanExtinguish() {
        
        Transform playerTransform = Camera.main.transform;
        return Physics.Raycast(playerTransform.position, playerTransform.forward, 4f, fireLayer);
        
    }
}
