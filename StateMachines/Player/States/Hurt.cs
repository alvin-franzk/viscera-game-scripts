using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt : PlayerBaseState
{
    protected PlayerMovementStateMachine _pmsm;

    public Hurt(PlayerMovementStateMachine stateMachine) : base(stateMachine) => _pmsm = (PlayerMovementStateMachine)stateMachine;

    public override void Enter()
    {
        base.Enter();

        _pmsm.animator.SetTrigger("isHurt");
        _pmsm.canDoAction = false;
        _pmsm.playerLookDirection.canLook = false;
        // play sound
        SoundManager.instance.PlaySoundRandom(_pmsm.hurtSound, _pmsm.transform, 0.3f);
        SoundManager.instance.PlaySoundRandom(_pmsm.gruntSound, _pmsm.transform, 0.4f);
        _pmsm.StartCoroutine(_pmsm.ResetCanDoAction());
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_pmsm.playerStatus.isDead)
        {
            _pmsm.ChangeState(_pmsm.dyingState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
