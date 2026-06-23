using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    public PlayerStateMachine stateMachine;
    public PlayerCore core;
    public MVMDefault movement;

    private void Start()
    {
        stateMachine.SwitchState(new IdleState(core, movement, stateMachine));
    }
}