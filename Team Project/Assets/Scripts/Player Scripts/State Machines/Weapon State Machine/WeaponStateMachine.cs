using UnityEngine;
using TMPro;
public class WeaponStateMachine : MonoBehaviour
{
    public WeaponState CurrentState { get; private set; }

    [SerializeField] private TextMeshProUGUI weaponStateText;
    public void SwitchState(WeaponState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
        weaponStateText.text = newState.ToString();
    }

    private void Update() => CurrentState?.Tick();
    private void FixedUpdate() => CurrentState?.FixedTick();
}
