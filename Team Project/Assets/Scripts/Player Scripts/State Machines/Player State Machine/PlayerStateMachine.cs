using TMPro;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }

    [SerializeField] private TextMeshProUGUI stateText;
    public void SwitchState(PlayerState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
        stateText.text = newState.ToString();
    }

    private void Update() => CurrentState?.Tick();
    private void FixedUpdate() => CurrentState?.FixedTick();
}
