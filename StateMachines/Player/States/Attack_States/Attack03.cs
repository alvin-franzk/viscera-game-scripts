using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack03 : Attacking
{
    public Attack03(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // Attack
        Debug.Log("Attack 3 Fired!");
        // _playerLookDirection.canLook = false;
        // attackDuration = 0.2f;
        // _pmsm.animator.SetTrigger("TriggerAttack03");
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        // canCombo = false;
        // _pmsm.ChangeState(_pmsm.idleState);

    }

    public override void Exit()
    {
        base.Exit();

        Debug.Log("Attack03 Exiting State");
    }
}
