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
    public WheelCollider wheelColliderLeftFront;
    public WheelCollider wheelColliderRightFront;
    public WheelCollider wheelColliderLeftBack;
    public WheelCollider wheelColliderRightBack;
    public Transform wheelLeftFront;
    public Transform wheelRightFront;
    public Transform wheelLeftBack;
    public Transform wheelRightBack;

    //  WheelCollider[] m_wheels;
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
    public float angle;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;

        gameplayActionMap = inputActions.FindActionMap("VehicleController");

        handBreakeInputAction = gameplayActionMap.FindAction("HandBrake");
        steeringInputAction = gameplayActionMap.FindAction("SteeringAngle");
        accelerationInputAction = gameplayActionMap.FindAction("Acceleration");

        handBreakeInputAction.Enable();
        steeringInputAction.Enable();
        accelerationInputAction.Enable();

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

    private void FixedUpdate()
    {

        wheelColliderLeftFront.ConfigureVehicleSubsteps(criticalSpeed, stepBelow, stepAbovve);
        wheelColliderRightFront.ConfigureVehicleSubsteps(criticalSpeed, stepBelow, stepAbovve);
        wheelColliderLeftBack.ConfigureVehicleSubsteps(criticalSpeed, stepBelow, stepAbovve);
        wheelColliderRightBack.ConfigureVehicleSubsteps(criticalSpeed, stepBelow, stepAbovve);

        if (wheelColliderLeftFront.transform.localPosition.z > 0 && wheelColliderRightFront.transform.localPosition.z > 0)
        {
            wheelColliderLeftFront.motorTorque = torque;
            wheelColliderRightFront.motorTorque = torque;
        }
        // if (wheelColliderLeftFront.transform.rotation.z > 0 && wheelColliderRightFront.transform.rotation.z > 0)
        // {
        //     wheelColliderLeftFront.steerAngle = angle;
        //     wheelColliderRightFront.steerAngle = angle;
        // }

        // Quaternion quat;
        // Vector3 pos;

        // wheelColliderLeftFront.GetWorldPose(out pos, out quat);
        // wheelLeftFront.position = pos;
        // wheelLeftFront.rotation = quat * Quaternion.Euler(0, 180, 0); ;

        // wheelColliderRightFront.GetWorldPose(out pos, out quat);
        // wheelRightFront.position = pos;
        // wheelRightFront.rotation = quat * Quaternion.Euler(0, 180, 0);

        // wheelColliderLeftBack.GetWorldPose(out pos, out quat);
        // wheelLeftBack.position = pos;
        // wheelLeftBack.rotation = quat;

        // wheelColliderRightBack.GetWorldPose(out pos, out quat);
        // wheelRightBack.position = pos;
        // wheelRightBack.rotation = quat;

        foreach (WheelCollider wheel in m_wheels)
        {
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

    }
    // private void OnDisable()
    // {
    //     handBreakeInputAction.Disable();
    //     steeringInputAction.Disable();
    //     accelerationInputAction.Disable();
    // }


}
