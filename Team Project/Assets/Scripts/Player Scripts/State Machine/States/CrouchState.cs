using UnityEngine;

public class CrouchState : PlayerState
{
    public CrouchState(PlayerCore core, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine sm) : base(core, movement, input, camera, sm) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);     
        movement.Crouch();
    }

    public override void Tick()
    {
        if(!input.Crouch) playerStateMachine.SwitchState(new IdleState(core, movement, input, camera, playerStateMachine));
    }

    public override void Exit()
    {
        movement.UnCrouch();        
    }
}
