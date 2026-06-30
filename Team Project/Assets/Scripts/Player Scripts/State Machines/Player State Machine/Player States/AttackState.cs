using UnityEngine;

public class AttackState : PlayerState
{
    public AttackState(PlayerCore playerCore, PlayerMovement movement, PlayerInput input, PlayerCamera camera, PlayerStateMachine psm, WeaponCore weaponCore) : base(playerCore, movement, input, camera, psm, weaponCore) { }

    public override void Enter()
    {
        Debug.Log("Entered" + playerStateMachine.CurrentState);
        //weaponCore.StartCoroutine(weaponCore.Attack());
    }

    public override void Tick()
    {
        
    }
}
