using System.Collections;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [HideInInspector] public CharacterController MController;

    [HideInInspector] public bool waitComplete = false;

    Vector3 GravityPull;
    public float GravityStrength;

    void Start()
    {
        MController = GetComponent<CharacterController>();
    }
    public void Update()
    {
        Gravity();
    }
    public void ToggleBool(ref bool boolToToggle)
    {
        boolToToggle = !boolToToggle;
    }

    public void LeaveStateAfterDelay(float stateDuration)
    {
        waitComplete = false;
        StartCoroutine(LeaveStateAfterDelayCoroutine(stateDuration));
    }
    private IEnumerator LeaveStateAfterDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        waitComplete = true;
    }

    public void Gravity()
    {
        GravityPull.y = -GravityStrength;
        MController.Move(GravityPull * Time.deltaTime);
    }
}
 