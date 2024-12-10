using UnityEngine;

public class CompassController : MonoBehaviour
{
    private bool increaseCollider;
    private SphereCollider DetectionCollider;
    private float RadiusCreasing;
    private GameObject ovejaTarget;
    public float CreasingSpeed;
    [SerializeField] private GameObject compassPointer;
    private bool brujulaActive;
    private AudioSource wistle;

    private void Start()
    {
        DetectionCollider = GetComponent<SphereCollider>();
        wistle = GetComponent<AudioSource>();
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

        compassPointer.transform.LookAt(new Vector3(ovejaTarget.transform.position.x, ovejaTarget.transform.position.y, ovejaTarget.transform.position.z));
    }


    public void Silbar()
    {
        AudioManager.Instance.StartWhistleSound = true;
        increaseCollider = true;
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Sheep"))
        {
            increaseCollider = false;
            ovejaTarget = other.gameObject;
            AudioManager.Instance.StartSheepSound = true;
        }
    }

    public void MostrarBrujula()
    {
        if (!brujulaActive) 
        {
            this.gameObject.SetActive(false);
            brujulaActive = true;
        }
        else if (brujulaActive)
        {
            this.gameObject.SetActive(true);
            brujulaActive = false;
        }
    }
}
