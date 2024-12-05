using UnityEngine;

public class Ladder : MonoBehaviour
{

    [SerializeField] private BoxCollider climbArea;


    public void PutLadder(Transform placeholderTransform)
    {
        
            transform.position = placeholderTransform.position;
            transform.rotation = placeholderTransform.rotation;
            gameObject.layer = LayerMask.NameToLayer("Climbable");
            climbArea.enabled = true;
    }


}
