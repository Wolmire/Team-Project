using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(PlayerCore core, MVMDefault movement, PlayerStateMachine sm) : base(core, movement, sm) { }
    public override void Enter()
    {
        Debug.Log("Entered" + stateMachine.CurrentState);
    }
    public override void Tick()
    {
        if (core.MoveInput.sqrMagnitude < 0.01f) stateMachine.SwitchState(new IdleState(core, movement, stateMachine));
    }
}
