using UnityEngine;

public class Blocking : PlayerBaseState
{
    private PlayerMovementStateMachine _pmsm;

    public float blockPenaltySpeed = 1.5f;

    public Blocking(PlayerMovementStateMachine stateMachine) : base(stateMachine) => _pmsm = (PlayerMovementStateMachine)stateMachine;

    public override void Enter()
    {
        base.Enter();

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // Transition to Idle State
        if (Input.GetKeyUp(KeyCode.Space) || Mathf.Floor(_pmsm.playerStatus.currentStamina) <= 0)
        {
            stateMachine.ChangeState(_pmsm.idleState);
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

        // Animation for Blocking
        _pmsm.animator.SetBool("isBlocking", true);

        // movement is nerfed IF blocking
        Vector3 movement = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // moving
        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= blockPenaltySpeed * Time.deltaTime;
            _pmsm.transform.Translate(movement, Space.World);
        }

        // animating | Running animation
        float velocityZ = Vector3.Dot(movement.normalized, _pmsm.transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, _pmsm.transform.right);

        _pmsm.animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        _pmsm.animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

        // REFER TO ANY Attack logic in enemy scripts (block logic is calculated there)
    }

    public override void Exit()
    {
        base.Exit();
        _pmsm.isBlocking = false;
        _pmsm.animator.SetBool("isBlocking", false);
    }
}
