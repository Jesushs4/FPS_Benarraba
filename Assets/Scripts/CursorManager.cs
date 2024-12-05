using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private Sprite dotIcon;
    [SerializeField] private Sprite grabIcon;
    [SerializeField] private Image cursor;
    [SerializeField] private Sprite clickIcon;

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
        if (playerMovement.IsGrabbing)
        {
            bool canPlaceLever = playerMovement.CheckLeverPlaceable() && playerMovement.ItemTransform.CompareTag("Lever");
            bool canPlaceLadder = playerMovement.CheckLadderPlaceable() && playerMovement.ItemTransform.CompareTag("Ladder");
            bool canPickLock = playerMovement.CheckCage() && playerMovement.ItemTransform.CompareTag("Lockpick");
            bool canExtinguish = playerMovement.ItemTransform.CompareTag("Extinguisher");
            if (canPlaceLadder || canPlaceLever || canPickLock)
            {
                cursor.sprite = clickIcon;
                return;
            } else if (canExtinguish)
            {
                if (playerMovement.ItemTransform.GetComponent<Extinguisher>().CanExtinguish())
                {
                    cursor.sprite = clickIcon;
                    return;
                }
            }
        }

        bool LeverPlaced = playerMovement.CheckLeverPlaceable() && playerMovement.LeverPlaced;

        if (LeverPlaced) {
            cursor.sprite = clickIcon;
            return;
        }

        bool LadderClimbable = playerMovement.LadderPlaced && playerMovement.CheckLadderPlaceable();

        if (LadderClimbable)
        {
            cursor.sprite = grabIcon;
            return;
        }

        if (playerMovement.CheckInteractuable())
        {
            cursor.sprite = grabIcon;
                return;
        }

        cursor.sprite = dotIcon;
    
    }
}
