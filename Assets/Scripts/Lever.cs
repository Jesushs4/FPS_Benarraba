using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private float rotationSpeed = 400f;
    private bool isRotating = false;

    public void PutLever(Transform placeholderTransform)
    {

        transform.position = placeholderTransform.position;
        transform.rotation = placeholderTransform.rotation;
    }

    public void RotateLever()
    {
        if (!isRotating) {
            isRotating = true;
            StartCoroutine(RotateLeverCoroutine());
        }
        
    }

    private IEnumerator RotateLeverCoroutine()
    {
        float totalRotation = 0f;
        while (totalRotation < 360f)
        {
            float parcialRotation = rotationSpeed * Time.deltaTime;

            if (totalRotation + parcialRotation > 360f)
            {
                parcialRotation = 360f - totalRotation;
            }

            transform.Rotate(0, 0, parcialRotation);
            totalRotation += parcialRotation;

            yield return null;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        isRotating = false;
    }

}
