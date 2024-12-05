using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Components
    private CharacterController characterController;
    private Transform cameraTransform;

    //movement and jump configuration parameters
    [SerializeField] private float speed = 5f;
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private float jumpForce = 1.5f;
    [SerializeField] private float gravity = Physics.gravity.y;
    [SerializeField] private Transform hand;

    //Input fields for movement and look actions
    private Vector2 moveInput;
    private Vector2 lookInput;

    //Velocity and rotation variables
    private Vector2 velocity;
    private float verticalVelocity;
    private float verticalRotation = 0;

    //Is Sprinting state
    private bool isSprinting;
    private bool isMoving;

    private LayerMask itemLayer;
    private Transform itemTransform;
    private bool isGrabbing;

    private LayerMask placeholderLadderLayer;
    private Transform placeholderLadderTransform;

    private LayerMask placeholderLeverLayer;
    private Transform placeholderLeverTransform;
    private Transform leverTransform;
    private bool leverPlaced = false;
    private bool isLockpicking = false;

    private LayerMask cageLayer;

    [SerializeField] private GameObject lockpickPanel;

    private LayerMask placeholderValveLayer;
    private Transform placeholderValveTransform;

    //Camera look sensitivity and max angle to limit vertical rotation
    [SerializeField] private float lookSentitivity = 1f;
    private float maxLookAngle = 80f;

    private bool isClimbing = false;
    [SerializeField] private float climbSpeed = 3f;
    private Transform currentLadder;

    private Lockpick lockpick;


    private bool ValvePlaced;
    private Transform ValveTransform;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        itemLayer = LayerMask.GetMask("Item");

        placeholderLadderLayer = LayerMask.GetMask("Ladder");
        placeholderLeverLayer = LayerMask.GetMask("Lever");
        cageLayer = LayerMask.GetMask("Cage");


        lockpick = lockpickPanel.GetComponent<Lockpick>();

        placeholderValveLayer = LayerMask.GetMask("Valve");

        //Hide mouse cursor
        Cursor.lockState = CursorLockMode.Locked;


    }

    private void Update()
    {
        isLockpicking = lockpickPanel.activeSelf;
        if (isClimbing)
        {
            ClimbLadder();
        }
        else
        {
            MovePlayer();
        }

        //Manage Camera Rotation
        LookAround();

    }

    /// <summary>
    /// Receives movement input from Input System
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        isMoving = moveInput != Vector2.zero;
    }

    /// <summary>
    /// Receive look input from the Input System
    /// </summary>
    /// <param name="context"></param>
    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (isLockpicking && context.started)
        {
            lockpick.HitNeedle();
            return;
        }
        if (context.started && currentLadder != null)
        {
            isClimbing = !isClimbing;

            if (!isClimbing)
            {
                verticalVelocity = 0f;
            }
        }

        if (CheckInteractuable() && context.started && !isGrabbing)
        {
            GrabObject();
        }
        else if (isGrabbing && context.started)
        {
            DropObject();
        }

    }

    public void Attack(InputAction.CallbackContext context)
    {

        if (isGrabbing && itemTransform.CompareTag("Extinguisher"))
        {
            Extinguisher extinguisher = itemTransform.GetComponent<Extinguisher>();
            if (extinguisher != null)
            {
                if (context.started)
                {
                    extinguisher.StartExtinguish();
                }
                else if (context.canceled)
                {
                    extinguisher.StopExtinguishing();
                }
            }
        }

        else if (leverPlaced && CheckLeverPlaceable() && context.started)
        {
            Lever lever = leverTransform.GetComponent<Lever>();
            lever.RotateLever();
        }

        else if (ValvePlaced && CheckValvePlaceable() && context.started)
        {
            Valve valve = ValveTransform.GetComponent<Valve>();
            valve.UseValve();
        }


        else if (context.started && isGrabbing)
        {
            UseObject();
        }
    }
    /// <summary>
    /// Receive Sprint input from Input System and change isSprinting state
    /// </summary>
    /// <param name="context"></param>
    public void Sprint(InputAction.CallbackContext context)
    {
        //when action started or mantained
        isSprinting = context.started || context.performed;

    }

    private void ClimbLadder()
    {

        Vector3 upwardDirection = Vector3.up;

        Vector3 lateralDirection = -Vector3.Cross(currentLadder.forward, upwardDirection).normalized;

        Vector3 climbMovement = upwardDirection * moveInput.y * climbSpeed;
        Vector3 lateralMovement = lateralDirection * moveInput.x * climbSpeed;

        Vector3 finalMovement = climbMovement + lateralMovement;

        characterController.Move(finalMovement * Time.deltaTime);

        if (moveInput == Vector2.zero)
        {
            verticalVelocity = 0f;
        }
    }


    /// <summary>
    /// Handles player movement and jump based on Input and applies gravity
    /// </summary>
    private void MovePlayer()
    {

        //Falling Down
        if (characterController.isGrounded)
        {
            //Restart vertical velocity when touch ground
            verticalVelocity = 0f;
        }
        else
        {
            //when is falling down increment velocity with gravity and time
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);

        float targetSpeed = isSprinting ? speed * multiplier : speed;
        Vector3 movement = moveDirection * targetSpeed + Vector3.up * verticalVelocity;

        characterController.Move(movement * Time.deltaTime);
    }

    /// <summary>
    /// Handles camera rotation based on Look Input 
    /// </summary>
    private void LookAround()
    {
        //Horizontal rotation (Y-axis) based on sensitivity and input
        float horizontalRotation = lookInput.x * lookSentitivity;
        transform.Rotate(Vector3.up * horizontalRotation);

        //Vertical rotation (X-axis) with clamping to prevent over-rotation
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    public bool CheckInteractuable()
    {
        float rayLength = 2f;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, rayLength, itemLayer))
        {
            itemTransform = hit.transform;
            return true;
        }
        return false;
    }

    private void GrabObject()
    {
        isGrabbing = true;
        itemTransform.SetParent(hand);
        itemTransform.GetComponent<Rigidbody>().isKinematic = true;
        itemTransform.position = hand.position;
        if (itemTransform.CompareTag("Extinguisher"))
        {
            itemTransform.localRotation = Quaternion.Euler(-90, hand.rotation.y, 90);
        } else if (itemTransform.CompareTag("Ladder"))
        {
            itemTransform.localRotation = Quaternion.Euler(hand.rotation.x, 60, 90);
        }
        else
        {
            itemTransform.rotation = hand.rotation;
        }
        
    }

    private void DropObject()
    {
        isGrabbing = false;
        itemTransform.GetComponent<Rigidbody>().isKinematic = false;
        itemTransform.SetParent(null);
        itemTransform = null;
    }

    private void UseObject()
    {
        if (itemTransform == null)
        {
            return;
        }


        if (itemTransform.CompareTag("Ladder") && CheckLadderPlaceable())
        {
            var ladder = itemTransform.GetComponent<Ladder>();
            if (ladder != null)
            {
                ladder.PutLadder(placeholderLadderTransform);
            }

            isGrabbing = false;
            itemTransform.SetParent(null);
            itemTransform = null;
            return;
        }


        if (itemTransform.CompareTag("Lever") && CheckLeverPlaceable())
        {
            var lever = itemTransform.GetComponent<Lever>();
            if (lever != null)
            {
                lever.PutLever(placeholderLeverTransform);
                leverTransform = itemTransform;
            }
        }

        if (itemTransform.CompareTag("Valve") && CheckValvePlaceable())
        {

            var valve = itemTransform.GetComponent<Valve>();

            if (valve != null)
            {
                valve.PutValve(placeholderValveTransform);
            }

            isGrabbing = false;
            itemTransform.SetParent(null);
            ValveTransform = itemTransform;
            itemTransform = null;
            ValvePlaced = true;
            return;
        }

        if (itemTransform.CompareTag("Lockpick") && CheckCage())
        {
            lockpickPanel.SetActive(true);
            lockpick.StartMinigame();
        }
    }



    private bool CheckLadderPlaceable()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, 4f, placeholderLadderLayer))
        {
            placeholderLadderTransform = hit.transform;
            return true;
        }
        return false;
    }

    private bool CheckLeverPlaceable()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, 4f, placeholderLeverLayer))
        {
            placeholderLeverTransform = hit.transform;
            return true;
        }
        return false;
    }
    private bool CheckValvePlaceable()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, 4f, placeholderValveLayer))
        {
            placeholderValveTransform = hit.transform;
            return true;
        }
        return false;
    }

    private bool CheckCage()
    {
        return Physics.Raycast(cameraTransform.position, cameraTransform.forward, 4f, cageLayer);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            currentLadder = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder") && other.transform == currentLadder)
        {
            currentLadder = null;
            isClimbing = false;
        }
    }
}

