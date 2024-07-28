using System.Collections;
using UnityEngine;

public class Sprinting : Moving
{
    public Sprinting(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        if (_pmsm.playerStatus.currentStamina >= 2)
        {
            _pmsm.animator.SetBool("isSprinting", true);
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        Vector3 movement = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // Transition to Idle State
        if (movement == Vector3.zero)
        {
            stateMachine.ChangeState(_pmsm.idleState);
        }
        // Transition to Dodging State
        if (Input.GetKeyDown(KeyCode.Space) && _pmsm.canDodge && Mathf.Floor(_pmsm.playerStatus.currentStamina) >= Mathf.Abs(_pmsm.dodgeCost))
        {
            _pmsm.canDodge = false;
            _pmsm.playerStatus.isInvulnerable = true;
            stateMachine.ChangeState(_pmsm.dodgingState);
        }
        // Transition to Moving State
        if (Input.GetKeyUp(KeyCode.LeftShift) || Mathf.Floor(_pmsm.playerStatus.currentStamina) <= 0)
        {
            _pmsm.animator.SetBool("isSprinting", false);
            stateMachine.ChangeState(_pmsm.movingState);
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

        Vector3 movement = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // moving
        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= _pmsm.sprintSpeed * Time.deltaTime;
            _pmsm.transform.Translate(movement, Space.World);
        }

        // animating | Running animation
        float velocityZ = Vector3.Dot(movement.normalized, _pmsm.transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, _pmsm.transform.right);

        _pmsm.animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        _pmsm.animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

        // sprinting costs stamina
        _pmsm.playerStatus.AdjustCurrentStamina(_pmsm.sprintCost * Time.deltaTime);

    }

    public override void Exit()
    {
        base.Exit();
    }
}
