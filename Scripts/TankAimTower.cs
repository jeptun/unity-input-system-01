using UnityEngine;

public class TankAimTower : MonoBehaviour
{

    [SerializeField] private Transform tankBarrel;
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
            float currentDisBetwMousPosx = (Input.mousePosition - PressPoint).x;
            transform.rotation = StartRotation * Quaternion.Euler(Vector3.up * (currentDisBetwMousPosx / SceenWidth) * 360);
            float currentDisBetwMousPosy = (Input.mousePosition - PressPoint).y;
            tankBarrel.rotation = StartRotation * Quaternion.Euler(Vector3.right * (currentDisBetwMousPosy / SceenWidth) * 20);
        }

    }

}

