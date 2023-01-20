using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




//
//
//Udelat console log na start stop inputu kdy se volaji a jestli skonci
//
//




public enum DriveType { RearWheelDrive, FrontWheelDrive, AllWheelDrive }
public class VehicleController : MonoBehaviour
{
    [SerializeField] float maxAngle = 30f;
    [SerializeField] float maxTorque = 30f;
    [SerializeField] float breakeTorque = 30000f;

    [SerializeField] GameObject wheeleShape;

    [SerializeField] float criticalSpeed = 5f;
    [SerializeField] int stepBelow = 5;
    [SerializeField] int stepAbovve = 1;

    [SerializeField] DriveType driveType;
    WheelCollider[] m_wheels;
    float handBreake, torque;
    public float angle;

    public InputActionAsset inputActions;
    InputActionMap gameplayActionMap;
    InputAction handBreakeInputAction;
    InputAction steeringInputAction;
    InputAction accelerationInputAction;



    void Awake()
    {
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
        for( int i = 0; i < m_wheels.Length; i++)
        {
            var wheel = m_wheels[i];
            if (wheeleShape != null)
            {
                var ws = Instantiate(wheeleShape);
                    ws.transform.parent = wheel.transform;
            }
        }
    }
    void Update()
    {
        m_wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepBelow, stepAbovve);

        foreach (WheelCollider wheel in m_wheels)
        {
            if (wheel.transform.localPosition.z > 0)
            {
                wheel.steerAngle = angle;
            }

            if (wheel.transform.localPosition.z < 0)
            {
                wheel.brakeTorque = handBreake;
            }

            if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheel.motorTorque = torque;
            }
            if (wheel.transform.localPosition.z > 0 && driveType != DriveType.RearWheelDrive)
            {
                wheel.motorTorque = torque;
            }

            if (wheeleShape)
            {
                Quaternion quat;
                Vector3 pos;

                wheel.GetWorldPose(out pos, out quat);

                Transform shapeTransform = wheel.transform.GetChild(0);

                if (wheel.name == "BvpWheelLeftFront" || wheel.name == "BvpWheelRightFront")
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
    }

    void GetAngleInput(InputAction.CallbackContext context)
    {
        angle = context.ReadValue<float>() * maxAngle;
    }

    void GetTorqueInput(InputAction.CallbackContext context)
    {
        torque = context.ReadValue<float>() * maxTorque;
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
