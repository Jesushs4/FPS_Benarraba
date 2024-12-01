using UnityEngine;

public class Fire : MonoBehaviour
{
    public float extinguishRate = 0.1f;
    private bool isExtinguished = false;

    public void Extinguish()
    {
        Debug.Log("ENTRA2");

        if (isExtinguished) return;

        transform.localScale -= Vector3.one * extinguishRate;

        if (transform.localScale.x <= 0.1f || transform.localScale.y <= 0.1f || transform.localScale.z <= 0.1f)
        {
            transform.localScale = Vector3.zero;
            isExtinguished = true;
            Destroy(gameObject);
        }
    }
}