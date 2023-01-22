using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComponentCaro : MonoBehaviour
{
    [Header("NASTAVENI VOZIDLA")]

    [Tooltip("Maximalni sila motoru")]
    [SerializeField] private float maxTorque = 1000f;
    [Tooltip("Maximalni natočení")]
    [SerializeField] private float maxAngle = 1000f;
    [Tooltip("Brzda")]
    [SerializeField] private float breakeTorque = 30000f;
    [Tooltip("Brzda")]

    [SerializeField] private Transform centerOfMass;


    private Rigidbody rb;

    [Header("NASTAVENI Kola")]

    [SerializeField] GameObject wheeleShape;
    WheelCollider[] m_wheels;
    // public WheelCollider wheelColliderLeftFront;
    // public WheelCollider wheelColliderRightFront;
    // public WheelCollider wheelColliderLeftBack;
    // public WheelCollider wheelColliderRightBack;

    [SerializeField] float criticalSpeed = 5f;
    [SerializeField] int stepBelow = 5;
    [SerializeField] int stepAbovve = 1;


    [Header("NASTAVENI Input ActionAsset")]
    public InputActionAsset inputActions;
    InputActionMap gameplayActionMap;
    InputAction handBreakeInputAction;
    InputAction steeringInputAction;
    InputAction accelerationInputAction;

    bool isMoving = false;
    bool disableMovement = false;
    private float handBreake, torque;
    private float angle;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;

        gameplayActionMap = inputActions.FindActionMap("VehicleController");

        handBreakeInputAction = gameplayActionMap.FindAction("HandBrake");
        steeringInputAction = gameplayActionMap.FindAction("SteeringAngle");
        accelerationInputAction = gameplayActionMap.FindAction("Acceleration");

        handBreakeInputAction.performed += GetHandBrakeInput;
        handBreakeInputAction.canceled += GetHandBrakeInput;

        steeringInputAction.performed += GetAngleInput;
        steeringInputAction.canceled += GetAngleInput;

        accelerationInputAction.performed += GetTorqueInput;
        accelerationInputAction.performed += GetTorqueInput;


    }

    private void Start()
    {
        m_wheels = GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < m_wheels.Length; i++)
        {
            var wheel = m_wheels[i];
            if (wheeleShape != null)
            {
                var ws = Instantiate(wheeleShape);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    private void Update()
    {

        m_wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepBelow, stepAbovve);


        foreach (WheelCollider wheel in m_wheels)
        {


            // if (wheel.transform.rotation.z > 0)
            // {
            //     wheel.steerAngle = angle;
            // }
            // if (wheel.transform.localPosition.z < 0)
            // {
            //     wheel.motorTorque = torque;
            // }
            // if (wheel.transform.localPosition.z > 0)
            // {
            //     wheel.motorTorque = torque;
            // }

            if (wheeleShape)
            {
                Quaternion quat;
                Vector3 pos;

                wheel.GetWorldPose(out pos, out quat);

                Transform shapeTransform = wheel.transform.GetChild(0);

                if (wheel.name == "BvpWheelLeftFrontMesh" || wheel.name == "BvpWheelRightFrontMesh")
                {

                    shapeTransform.rotation = quat * Quaternion.Euler(0, 180, 0);
                    shapeTransform.position = pos;

                }
                else
                {
                    shapeTransform.rotation = quat;
                    shapeTransform.position = pos;
                }
            }

        }

    }
    void GetHandBrakeInput(InputAction.CallbackContext context)
    {
        handBreake = context.ReadValue<float>() * breakeTorque;
        Debug.Log("GetHandBrakeInput metoda");
    }

    void GetAngleInput(InputAction.CallbackContext context)
    {
        angle = context.ReadValue<float>() * maxAngle;
        Debug.Log("GetAngleInput metoda");
    }

    void GetTorqueInput(InputAction.CallbackContext context)
    {
        torque = context.ReadValue<float>() * maxTorque;
        Debug.Log("GetTorqueInput metoda");
    }


    private void OnEnable()
    {
        handBreakeInputAction.Enable();
        steeringInputAction.Enable();
        accelerationInputAction.Enable();

    }
    private void OnDisable()
    {
        handBreakeInputAction.Disable();
        steeringInputAction.Disable();
        accelerationInputAction.Disable();
    }


}
