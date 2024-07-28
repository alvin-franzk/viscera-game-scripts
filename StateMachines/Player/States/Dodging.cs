using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodging : Moving
{  
    public Dodging(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        // Dodge cost
        _pmsm.playerStatus.AdjustCurrentStamina(_pmsm.dodgeCost);
        _pmsm.animator.SetTrigger("TriggerDodge");

        // play audio
        SoundManager.instance.PlaySoundRandom(_pmsm.dodgeSound, _pmsm.transform, 0.3f);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        Vector3 movement = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (movement != Vector3.zero)
        {
            stateMachine.ChangeState(_pmsm.movingState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        
    }

    public override void Exit()
    {
        base.Exit();
        SoundManager.instance.PlaySoundRandom(_pmsm.landSound, _pmsm.transform, 0.2f);
        _pmsm.StartCoroutine(_pmsm.ResetDodgeInvulnerability());
        _pmsm.StartCoroutine(_pmsm.ResetDodgeCooldown());
    }
}
