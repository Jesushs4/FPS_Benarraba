using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private Sprite dotIcon;
    [SerializeField] private Sprite grabIcon;
    [SerializeField] private Image cursor;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if (playerMovement.CheckInteractuable())
        {
            cursor.sprite = grabIcon;
            return;
        }
        cursor.sprite = dotIcon;
    }
}
