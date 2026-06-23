using UnityEngine;

public abstract class B_MVLook: MonoBehaviour
{
    public bool Locked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void RotationManager(Vector2 LookInput)
    {

    }

    public virtual void PlayerManager(Vector3 PlayerVelocity)
    {

    }
    public abstract void ChangeLookMode();
}
