using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovementScript : MonoBehaviour
{
    //Variables
    [Header("Look")]
    public float minYLook;
    public float maxYLook;
    public float mouseMultiplier;

    [Header("Movement")]
    public float speedMultiplier;
    public float scooterMultiplier;

    private Vector2 movementInput = Vector2.zero;
    private Vector2 mouseInput = Vector2.zero;
    private float cameraYRotation;
    private bool isInteracting = false;
    private bool canEnterScooter = true;
    public bool hasMetTheMayor = false;


    private GameObject currentScooter = null;

    //References
    [Header("References")]
    [SerializeField] private GameObject cameraObject;
    private CharacterController playerController;


    //Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //Get components
        playerController = GetComponent<CharacterController>();
    }


    //Update is called once per frame
    void Update()
    {
        //If the menu is not active, allow move and rotation
        if (ButtonManager.buttonManager != null)
        {
            if (!ButtonManager.buttonManager.menu && hasMetTheMayor)
            {
                Move();
                Rotate();
            }
        }
        else //If the button manager is not present, just allow the move and rotation
        {
            Move();
            Rotate();
        }

        //If the player is in a scoot, update it's position and the speed
        if (currentScooter != null) ScooterUpdate();
    }


    //Move the player 
    private void Move()
    {
        //Vectors depending on player's rotation
        Vector3 localForward = transform.TransformDirection(Vector3.forward);
        Vector3 localRight = transform.TransformDirection(Vector3.right);

        //If the player is traveling on scooter, don't allow lateral move
        if (currentScooter != null) movementInput.x = 0f;

        //Combine moves
        Vector3 combinedMoves = ((localForward * movementInput.y) + (localRight * movementInput.x)) * speedMultiplier;

        //If the player is traveling in scooter multiply it's speed by the scooter speed
        if (currentScooter != null) combinedMoves = combinedMoves * scooterMultiplier;

        //Move in both axis
        playerController.SimpleMove(combinedMoves);
    }


    //Rotate the player and the camera
    private void Rotate()
    {
        //Rotate the player along Y axis using mouse X position
        transform.Rotate(0, mouseInput.x * mouseMultiplier, 0);

        //Rotate the camera along X axis using mouse Y position
        cameraYRotation += mouseInput.y * mouseMultiplier;
        cameraYRotation = Mathf.Clamp(cameraYRotation, minYLook, maxYLook);
        cameraObject.transform.localEulerAngles = new Vector3(-cameraYRotation, 0, 0);
    }


    //Update scooters position and rotation
    private void ScooterUpdate()
    {
        //Update scooter's position
        currentScooter.transform.position = new Vector3(
            playerController.transform.position.x,
            playerController.transform.position.y - 1f,
            playerController.transform.position.z);

        //If the player is moving and the scooter's rotation is not already player's one, rotate scooter to player
        if (playerController.velocity.magnitude > 0.01f && currentScooter.transform.rotation != transform.rotation)
        {
            currentScooter.transform.DORotateQuaternion(transform.rotation, 0.5f);
        }
            
    }


    //Event => When pressing WASD, arrows, etc keys, move the camera
    public void OnMoveKeysInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }


    //Event => When moving the mouse, rotate the player's camera
    public void OnMoveMouseInput(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }


    //Event => When pressing E, interact or take an scooter if the player is near it
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        isInteracting = context.ReadValueAsButton();
    }


    //When stays in a trigger
    private void OnTriggerStay(Collider collider)
    {
        //Turn on prompt on the scooter
        if (collider.gameObject.CompareTag("Scooter") && currentScooter == null)
        {
            PromptScript promptScript = collider.gameObject.GetComponentInChildren<PromptScript>();

            //Activate the prompt only if the player is not too close from it
            if (Vector3.Distance(transform.position, collider.transform.position) >= 1.15f) promptScript.SetActive(true);
            else promptScript.SetActive(false);
        }

        //If the player has a scooter near, get it if presses E
        if (collider.gameObject.CompareTag("Scooter") && isInteracting && canEnterScooter && currentScooter == null)
        {
            Debug.Log("Enter scooter");

            //Deactivate prompt
            PromptScript promptScript = collider.gameObject.GetComponentInChildren<PromptScript>();
            promptScript.SetActive(false);

            StartCoroutine(ScooterStateDebouncer());

            currentScooter = collider.gameObject;

            //Set layer as 8 so it won't clip trough buildings
            currentScooter.layer = 8;
        }

        //If the player is already on scooter, get out of it if presses E
        if (collider.gameObject.CompareTag("Scooter") && isInteracting && !canEnterScooter && currentScooter != null)
        {
            Debug.Log("Exit scooter");
            StartCoroutine(ScooterStateDebouncer());

            //Restore layer
            currentScooter.layer = 0;
            currentScooter = null;
        }
    }


    //When exits a trigger
    private void OnTriggerExit(Collider collider)
    {
        //Turn on prompt on the scooter
        if (collider.gameObject.CompareTag("Scooter"))
        {
            PromptScript promptScript = collider.gameObject.GetComponentInChildren<PromptScript>();
            promptScript.SetActive(false);
        }
    }


    //Coroutine => Debounce the scooter state, the player can enter and exit properly
    private IEnumerator ScooterStateDebouncer()
    {
        yield return new WaitForSeconds(0.5f);

        canEnterScooter = !canEnterScooter;
    }
 }
