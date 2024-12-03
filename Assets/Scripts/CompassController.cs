using UnityEngine;

public class CompassController : MonoBehaviour
{
    private bool increaseCollider;
    private SphereCollider DetectionCollider;
    private float RadiusCreasing;
    private GameObject ovejaTarget;
    public float CreasingSpeed;
    [SerializeField] private GameObject compassPointer;


    private void Start()
    {
        DetectionCollider = GetComponent<SphereCollider>();
    }
    void Update()
    {

        if (increaseCollider)
        {
            RadiusCreasing += CreasingSpeed * Time.deltaTime;
        }
        else if (!increaseCollider)
        {
            RadiusCreasing = 0;
        }

        DetectionCollider.radius = RadiusCreasing;

        compassPointer.transform.LookAt(ovejaTarget.transform);
    }


    public void Silbar()
    {
        increaseCollider = true;
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Sheep"))
        {
            increaseCollider = false;
            ovejaTarget = other.gameObject;
        }
    }
}
