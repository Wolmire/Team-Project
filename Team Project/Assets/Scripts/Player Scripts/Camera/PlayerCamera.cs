using System.Net.Sockets;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject Camera;

    [Header("Sensitivity")]
    public float XSensitivity = 1;
    public float YSensitivity = 1;

    [Header("Limits")]
    public float NegativeXLimit;
    public float PositiveXlimit;

    public bool Locked = false;

    float xRotation;
    float yRotation;

    public void RotationManager(Vector2 LookInput)
    {
        xRotation -= LookInput.y * XSensitivity;
        yRotation += LookInput.x * YSensitivity;
        xRotation = Mathf.Clamp(xRotation, NegativeXLimit, PositiveXlimit);

        if (Locked)
        {
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
            Camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
        else
        {
            Camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }
    public void PlayerRotManager(Vector3 PlayerVelocity)
    {
        if (!Locked)
        {
            if(PlayerVelocity.magnitude > 0.1f)
            { 
            Vector3 RefinedPlayerVelocity = new Vector3(PlayerVelocity.x, 0, PlayerVelocity.z);
            transform.rotation = Quaternion.LookRotation(RefinedPlayerVelocity, Vector3.up);
            }
        }
    }
}
