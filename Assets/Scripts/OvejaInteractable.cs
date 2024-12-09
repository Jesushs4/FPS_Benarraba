using UnityEngine;

public class OvejaInteractable : MonoBehaviour
{

    private Transform cameraTransform;
    [SerializeField] private ParticleSystem Explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SheepCounter()
    {
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Explosion.Play();
        GameManager.Instance.SheepCounter--;
        Destroy(gameObject);
    }
}
