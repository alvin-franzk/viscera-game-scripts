using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack01 : Attacking
{
    public Attack01(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // Attack
        // Debug.Log("Attack 1 Fired!");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // TODO: Do attack combo logic

        // TODO: Attack buffer; let animation finish before moving to attack02
        /*
        _pmsm.playerLookDirection.canLook = false;
        _pmsm.animator.SetTrigger("TriggerAttack01");

        if (Input.GetMouseButtonDown(0) && canDoTemp)
        {
            lastClickTime = Time.time; // Update the last click time
            comboStep++;

            if (Time.time - lastClickTime > _comboResetTime)
            {
                comboStep = 0;
                Debug.Log("Combo Window Over");
                stateMachine.ChangeState(_pmsm.idleState);
            }
        }
        */
        
    }

    public override void Exit()
    {
        base.Exit();
        
        // Debug.Log("Attack01 Exiting State");
    }
}
