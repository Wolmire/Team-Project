using UnityEngine;

public class LedgeState : PlayerState
{
    public LedgeState(PlayerCore playerCore, PlayerMovement movement, PlayerInputManager input, PlayerStateMachine psm, WeaponCore weaponCore, TargetLockHandler targetLock, PlayerEquipManager equipManager) : base(playerCore, movement, input, psm, weaponCore, targetLock, equipManager) { }
    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        movement.MController.enabled = false;
        movement.LedgeInitiate();
        movement.SetIKFromHere(false);

    }

    // Update is called once per frame
    public override void Tick()
    {
        if(input.MoveInput.y < -0.49f) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        if(movement.OnLedge == false) playerStateMachine.SwitchState(new IdleState(playerCore, movement, input, playerStateMachine, weaponCore, targetLock, equipManager));
        movement.LedgeMove(input.MoveInput);
    }
    public override void Exit()
    {
        movement.MController.enabled = true;
        movement.SetIKFromHere(true);
        movement.OnLedge = false;
        movement.AnimationBool("OnLedge", false);
    }
}
