using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Valve : MonoBehaviour
{
    private bool isRotating = false;
    private bool fountainOff;
    [SerializeField] private ParticleSystem fountain;
    [SerializeField] private GameObject fountainSheep;

    private void Update()
    {
        #region FountainAudio

        if (!fountainOff)
        {
            AudioManager.Instance.FountainAudio.enabled = true;

        }
        else
        {
            AudioManager.Instance.FountainAudio.enabled = false;
        }

        #endregion
    }

    public void PutValve (Transform placeholderTransform)
    {

        transform.position = placeholderTransform.position;
        transform.rotation = placeholderTransform.rotation;
        gameObject.layer = LayerMask.GetMask("Default");
        placeholderTransform.gameObject.GetComponent<Renderer>().enabled = false;
    }

    public void UseValve()
    {
        if (!isRotating)
        {
            isRotating = true;
            Debug.Log("Rotao");
            transform.DORotate(
           new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 360, transform.rotation.eulerAngles.z),
           1f,
           RotateMode.FastBeyond360)
           .OnComplete(() =>
           {
               isRotating = false;
           });
        }

        var emission = fountain.emission;
         emission.rateOverTime = 0f;

        fountainSheep.GetComponent<Rigidbody>().isKinematic = false;

    }
}
