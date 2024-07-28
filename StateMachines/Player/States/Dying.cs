using UnityEngine;

public class Dying : PlayerBaseState
{
    private PlayerMovementStateMachine _pmsm;

    public Dying(PlayerMovementStateMachine stateMachine) : base(stateMachine) => _pmsm = (PlayerMovementStateMachine)stateMachine;

    public override void Enter()
    {
        base.Enter();
        _pmsm.animator.SetBool("isDead", true);
        _pmsm.playerLookDirection.canLook = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();

        SoundManager.instance.PlaySound(_pmsm.deathSound, _pmsm.transform, 0.8f);
    }
}
