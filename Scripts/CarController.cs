using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour

{
    [SerializeField] float motorThrust = 100f;
    Vector2 moveDirection= Vector2.zero;
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleInput();
        MoveUp();
    }
        private void HandleInput()
    {
        moveDirection = InputManager.instance.GetMoveDirection();
    }

    public void MoveUp()
    {
        rb.AddRelativeForce(Vector3.up * motorThrust * Time.fixedDeltaTime);
    }

   
    //public float speed;
    //private Vector2 move;

    //public void OnMove(InputAction.CallbackContext context)
    //{
    //    move= context.ReadValue<Vector2>();
    //}

    //private void Update()
    //{
    //    movePlayer();
    //}

    //private void movePlayer()
    //{
    //        Vector3 movement = new Vector3(move.x, 0f, move.y);

    //        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    //}

    //[Header("Movement Params")]
    //[SerializeField] private float runSpeed = 6.0f;

    //private BoxCollider box;
    //private Rigidbody rb;

    ///*
    // * parametr pro input movement
    // */
    //Vector2 moveDirection = Vector2.zero;

    //private Vector2 move;
    //private void Awake()
    //{
    //    box = GetComponent<BoxCollider>();
    //    rb = GetComponent<Rigidbody>();
    //    //  move= GetComponent<Vector2>();
    //}

    //// Update is called once per frame
    //public void Update()
    //{
    //    HandleInput();
    //    HandleMovement();
    //}
    //private void HandleInput()
    //{
    //    moveDirection = InputManager.instance.GetMoveDirection();
    //}
    //public void HandleMovement()
    //{
    //    moveDirection = InputManager.instance.GetMoveDirection();
    //    Vector3 vector3 = new Vector3(moveDirection.x, 0f, moveDirection.y);

    //    transform.Translate(vector3 * runSpeed * Time.deltaTime, Space.World);

    //    // rb.velocity = new Vector2(moveDirection.x * runSpeed, rb.velocity.y);
    //    // rb.AddForce(new Vector3(moveDirection.x, 0, moveDirection.y) * runSpeed, ForceMode.Force);
    //}

}
