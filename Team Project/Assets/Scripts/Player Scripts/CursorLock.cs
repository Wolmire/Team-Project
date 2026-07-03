using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLock : MonoBehaviour
{
    public bool cursorLocked = false;
    void Start()
    {
        LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }

    public void ToggleCursorLock()
    {
        if (cursorLocked) UnlockCursor();
        else LockCursor();
    }
}
