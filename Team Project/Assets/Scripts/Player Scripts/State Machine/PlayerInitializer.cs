using UnityEngine;
public class PlayerInitializer : MonoBehaviour
{
    public PlayerStateMachine stateMachine;
    public PlayerCore core;
    public PlayerMovement movement;
    public PlayerInput input;
    public PlayerCamera camera;

    private void Start()
    {
        stateMachine.SwitchState(new IdleState(core, movement, input, camera, stateMachine));
    }
}