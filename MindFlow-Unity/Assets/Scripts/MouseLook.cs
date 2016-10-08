using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{
    public float LookSpeed = 1f;
    public float MinAngleX = -60f;
    public float MaxAngleX = 60f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 angles = transform.localEulerAngles;
        if (angles.x > 180f)
            angles.x -= 360f;
        angles.y += Input.GetAxis("Mouse X") * LookSpeed * Time.deltaTime;
        angles.x -= Input.GetAxis("Mouse Y") * LookSpeed * Time.deltaTime;
        if (angles.x < MinAngleX)
            angles.x = MinAngleX;
        else if (angles.x > MaxAngleX)
            angles.x = MaxAngleX;
        transform.localRotation = Quaternion.Euler(angles);

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
