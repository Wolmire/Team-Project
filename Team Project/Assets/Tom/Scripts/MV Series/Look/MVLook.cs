using UnityEngine;

public abstract class MVLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void RotationManager(Vector2 LookInput)
    {
    }
    public abstract void ChangeLookMode();
}
