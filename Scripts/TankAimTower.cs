using UnityEngine;

public class TankAimTower : MonoBehaviour
{
    private float SceenWidth;
    private Vector3 PressPoint;
    private Quaternion StartRotation;

    private void Start()
    {
        SceenWidth = Screen.width;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PressPoint = Input.mousePosition;
            StartRotation = transform.rotation;
        }
        else if (Input.GetMouseButton(1))
        {
            float currentDisBetwMousPos = (Input.mousePosition - PressPoint).x;
            transform.rotation = StartRotation * Quaternion.Euler(Vector3.up * (currentDisBetwMousPos / SceenWidth) * 360);
        }
    }

}

