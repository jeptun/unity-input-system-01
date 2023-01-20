using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private Vector2 moveDirection;


    public static InputManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }

        else if (context.canceled)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

}

//private Rigidbody rbody;
//private PlayerInput playerInput;
//private PlayerInputActions playerInputActions;


//private void Awake()
//{
//    rbody = GetComponent<Rigidbody>();
//    playerInput = GetComponent<PlayerInput>();

//    //    playerInputActions = new PlayerInputActions();
//    //    playerInputActions.Player.Enable();
//}

// Update is called once per frame
//void FixedUpdate()
//{
//    MovedForw();

//}

//public void MovePressed(InputAction.CallbackContext context)
//{
//    Vector2 inputVector = context.ReadValue<Vector2>();
//    float speed = 5f;
//    rbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
//}

//private void MovedForw()
//{
//    rbody.velocity = new Vector3(0, 1, 0);
//}

