using UnityEngine;

public class MVLDefault : MVLook
{
    //public float Sensitivity;
    float xRotation;
    float yRotation;
    public GameObject Camera;

    [Header("Limits")]
    public float NegativeXLimit;
    public float PositiveXlimit;

    [Header("Sensitivity")]
    public float XSensitivity = 1;
    public float YSensitivity = 1;
    public override void RotationManager(Vector2 LookInput)
    {
        xRotation -= LookInput.y * XSensitivity;
        yRotation += LookInput.x * YSensitivity;
        xRotation = Mathf.Clamp(xRotation, NegativeXLimit, PositiveXlimit);

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        Camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
    public override void ChangeLookMode()
    {
        
    }
}
