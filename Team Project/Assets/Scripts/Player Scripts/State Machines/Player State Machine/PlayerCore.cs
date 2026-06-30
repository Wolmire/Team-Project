using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [HideInInspector] public CharacterController MController;

    [HideInInspector] public bool waitComplete = false;

    Vector3 GravityPull;
    public float GravityStrength;

    public float maxStamina = 100f;
    public float staminaRegenRate = 10f;
    public float currentStamina;
    public float staminaRegenRateMuliplier = 1f;

    public float idleStaminaRegenMultiplier = 1;
    public float walkStaminaRegenMultiplier = 0.8f;
    public float crouchStaminaRegenMultiplier = 1f;
    public float crouchWalkStaminaRegenMultiplier = 1f;
    public float runStaminaCost = 10f;
    public float runMinStamina = 15f;

    public bool isCrouching = false;

    public TextMeshProUGUI staminaText;
    void Start()
    {
        MController = GetComponent<CharacterController>();
    }
    public void Update()
    {
        Gravity();
        staminaText.text = currentStamina.ToString("F0") + " / " + maxStamina.ToString();
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
 