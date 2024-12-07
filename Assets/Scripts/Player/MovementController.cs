using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private Vector2 mouseDelta;

    [Header("Look")]
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpForce;
    public bool jump;

    //components
    private Rigidbody rig;

    private void Awake()
    {
        //get our components
        rig = GetComponent<Rigidbody>();
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (ButtonManager.buttonManager != null)
        {
            if (!ButtonManager.buttonManager.menu)
                CameraLook();
        }
        else
        {
            CameraLook();
        }

    }

    private void CameraLook()
    {
        //rotate the camera container up and down
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        Camera.main.transform.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        //rotate the palyer left and right
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    //called when we press wasd - managed by the Input System
    public void onMoveInput(InputAction.CallbackContext context)
    {
        //are we holding down a movement button?
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }

        //have we let go of a movement button?
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //calculate the move direction relative to where we're facing
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rig.linearVelocity.y;

        //assign our Rigibody velocity
        rig.linearVelocity = dir;
    }


    public void OnJumpInput(InputAction.CallbackContext context)
    {
        //is this the first frame we're pressing the button?
        if (context.phase == InputActionPhase.Started)
        {
            if (!jump)
            {
                rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jump = true;
                Invoke(nameof(JumpEnabled), 1f);
            }
            
        }
    }

    private void JumpEnabled()
    {
        jump = false;
    }
}
