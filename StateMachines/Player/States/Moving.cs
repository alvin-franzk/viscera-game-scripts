using System.Collections;
using UnityEngine;

public class Moving : PlayerBaseState
{
    protected PlayerMovementStateMachine _pmsm;
    protected float _horizontalInput;
    protected float _verticalInput;

    public Moving(PlayerMovementStateMachine stateMachine) : base(stateMachine) => _pmsm = (PlayerMovementStateMachine)stateMachine;

    public override void Enter()
    {
        base.Enter();
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
        
        // Check if Player has enough stamina for action
        if ((int)_pmsm.playerStatus.currentStamina != 0)
        {

            // Transition to Dodging State
            if (Input.GetKeyDown(KeyCode.Space) && _pmsm.canDodge && Mathf.Floor(_pmsm.playerStatus.currentStamina) >= Mathf.Abs(_pmsm.dodgeCost))
            {
                _pmsm.canDodge = false;
                _pmsm.playerStatus.isInvulnerable = true;
                stateMachine.ChangeState(_pmsm.dodgingState);
            }
            // Transition to Sprinting State
            if (Input.GetKey(KeyCode.LeftShift) && Mathf.Floor(_pmsm.playerStatus.currentStamina) >= Mathf.Abs(_pmsm.sprintCost))
            {
                stateMachine.ChangeState(_pmsm.sprintingState);
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

        // Debug.Log("Moving State");

        Vector3 movement = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // moving
        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= _pmsm.speed * Time.deltaTime;
            _pmsm.transform.Translate(movement, Space.World);
        }

        // animating | Running animation
        float velocityZ = Vector3.Dot(movement.normalized, _pmsm.transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, _pmsm.transform.right);

        _pmsm.animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        _pmsm.animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

        // play audio
        // SoundManager.instance.PlaySoundRandom(_pmsm.runSound, _pmsm.transform, 1f);
    }

    public override void Exit()
    {
        base.Exit();

        // Debug.Log("Moving Exiting State");
    }
}
