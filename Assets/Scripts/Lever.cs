using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private bool isRotating = false;

    public void PutLever(Transform placeholderTransform)
    {

        transform.position = placeholderTransform.position;
        transform.rotation = placeholderTransform.rotation;
        gameObject.layer = LayerMask.GetMask("Default");
    }

    public void RotateLever()
    {
        if (!isRotating)
        {
            isRotating = true;
            transform.DORotate(
                new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 360),
                1f,
                RotateMode.FastBeyond360) 
                .OnComplete(() =>
                {
                    isRotating = false; 
                });

        }
    }


}
