using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack02 : Attacking
{
    public Attack02(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // Attack
        Debug.Log("Attack 2 Fired!");
        // _playerLookDirection.canLook = false;
        //attackDuration = 0.2f;
        //_pmsm.animator.SetTrigger("TriggerAttack02");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // TODO: Do attack combo logic
        /*if (fixedTime >= attackDuration)
        {
            if (canCombo)
            {
                _pmsm.ChangeState(_pmsm.attack02State);
            }
        }
        else
        {
            canCombo = false;
            _pmsm.ChangeState(_pmsm.idleState);
        }*/
    }

    public override void Exit()
    {
        base.Exit();

        Debug.Log("Attack02 Exiting State");
    }
}
