using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : PlayerBaseState
{
    protected PlayerMovementStateMachine _pmsm;

    #region Movement Settings
    protected static float _horizontalInput;
    protected static float _verticalInput;
    #endregion

    public Idle(PlayerMovementStateMachine stateMachine) : base(stateMachine) => _pmsm = (PlayerMovementStateMachine)stateMachine; 

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_pmsm.canDoAction)
        {
            Vector3 movement = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // Transition to Moving State
            if (movement != Vector3.zero)
            {
                stateMachine.ChangeState(_pmsm.movingState);
            }

            if (Mathf.Floor(_pmsm.playerStatus.currentStamina) != 0)
            {
                // Transition to Blocking State
                if (Input.GetKey(KeyCode.Space))
                {
                    _pmsm.isBlocking = true;
                    stateMachine.ChangeState(_pmsm.blockingState);
                }

                // Transition to Attacking State
                if (Input.GetMouseButtonDown(0) && movement == Vector3.zero && _pmsm.playerStatus.currentStamina >= Mathf.Abs(_pmsm.attackCost))
                {
                    stateMachine.ChangeState(_pmsm.attackingState);
                }
            }
        }

        // Transition to Death State
        if (_pmsm.playerStatus.isDead)
        {
            stateMachine.ChangeState(_pmsm.dyingState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        // input is here to bring floats (velocityZ, velocityX) back to zero
        // if removed, input from Moving State will inherit towards Idle State
        // making the running animation play all the time

        // animating | Idle animation

        Vector3 movement = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        float velocityZ = Vector3.Dot(movement.normalized, _pmsm.transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, _pmsm.transform.right);

        _pmsm.animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        _pmsm.animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
