using UnityEngine;
using UnityEngine.UI;

public class Lockpick : MonoBehaviour
{
    [SerializeField] private Image hitArea;
    [SerializeField] private Image needle;

    [SerializeField] private float rotationSpeed = 50f;

    void Start()
    {
        if (hitArea != null)
        {
            SetRandomAngle();
        }
    }

    private void Update()
    {
        RotateNeedle();
    }

    void SetRandomAngle()
    {
        float randomAngle = Random.Range(0f, 360f);

        hitArea.transform.rotation = Quaternion.Euler(0, 0, -randomAngle);
    }

    private void RotateNeedle()
    {
        needle.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}
