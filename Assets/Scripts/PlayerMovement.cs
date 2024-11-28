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
    private float verticalRotation=0;

    //Is Sprinting state
    private bool isSprinting;
    private bool isMoving;

    private LayerMask itemLayer;
    private Transform itemTransform;
    private bool isGrabbing;

    //Camera look sensitivity and max angle to limit vertical rotation
    [SerializeField] private float lookSentitivity = 1f;
    private float maxLookAngle = 80f;



    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        itemLayer = LayerMask.GetMask("Item");

        //Hide mouse cursor
        Cursor.lockState = CursorLockMode.Locked;


    }

    private void Update()
    {
        //Manage Player Movement
        MovePlayer();

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
        if (CheckInteractuable() && context.started && !isGrabbing)
        {
            GrabObject();
        } else if (isGrabbing && context.started)
            {
                DropObject();
            }



    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && isGrabbing)
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

        Vector3 move = new Vector3(0, verticalVelocity,0);
        characterController.Move(move * Time.deltaTime);

        //Movement 
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);
        float targetSpeed = isSprinting ? speed * multiplier: speed;
        characterController.Move(moveDirection * targetSpeed * Time.deltaTime);

        //Apply gravity constantly to posibility Jump
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
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
        verticalRotation -= lookInput.y * lookSentitivity;
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
        itemTransform.rotation = hand.rotation;
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
        if (itemTransform.CompareTag("Item")) {
            itemTransform.GetComponent<ItemTest>().UseTest();
        }
        
    }


}
