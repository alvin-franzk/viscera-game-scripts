using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : PlayerBaseState
{
    private PlayerMovementStateMachine _pmsm;

    public int comboStep = 0;
    protected float lastClickTime;
    protected float _comboResetTime; // Time in seconds to reset the combo

    public Attacking(PlayerMovementStateMachine stateMachine) : base(stateMachine) => _pmsm = (PlayerMovementStateMachine)stateMachine;

    public override void Enter()
    {
        base.Enter();

        _pmsm.canDoAction = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // Transition to Death State
        if (_pmsm.playerStatus.isDead)
        {
            stateMachine.ChangeState(_pmsm.dyingState);
        }

        if (!_pmsm.isAttacking)
        {
            if (comboStep > 2)
            {
                comboStep = 0;
            }
            _comboResetTime = _pmsm.slashCooldown[comboStep] + 0.3f;
            // _comboResetTime = 2f;
            lastClickTime = Time.time; // Update the last click time
            comboStep++;
            // NOTE: Original HandleAttack code moved into function
            HandleAttack();

            if (Time.time - lastClickTime > _comboResetTime)
            {
                comboStep = 0;
                // Debug.Log("Combo Window Over");
                // stateMachine.ChangeState(_pmsm.idleState);
            }
        }

        

        stateMachine.ChangeState(_pmsm.idleState);
    }

    public override void Exit()
    {
        base.Exit();
   
    }

    void HandleAttack()
    {
        if (comboStep > 3) // Resets back to 1 if full 3-combo animation is achieved
        {
            comboStep = 1;
        }

        switch (comboStep)
        {
            case 1:
                _pmsm.animator.SetTrigger("TriggerAttack01");
                _pmsm.playerStatus.AdjustCurrentStamina(_pmsm.attackCost);
                break;
            case 2:
                _pmsm.animator.SetTrigger("TriggerAttack02");
                _pmsm.playerStatus.AdjustCurrentStamina(_pmsm.attackCost);
                break;
            case 3:
                _pmsm.animator.SetTrigger("TriggerAttack03");
                _pmsm.playerStatus.AdjustCurrentStamina(_pmsm.attackCost);
                break;
        }
        SoundManager.instance.PlaySoundRandom(_pmsm.attackSound, _pmsm.transform, 0.7f);
        _pmsm.StartCoroutine(AttackCooldown(_pmsm.slashCooldown[comboStep - 1] / 2));
    }

    private IEnumerator AttackCooldown(float duration)
    {
        // Debug.Log("Attack Cooldown: " + duration);
        _pmsm.isAttacking = true;
        _pmsm.playerLookDirection.canLook = false;
        yield return new WaitForSeconds(duration);
        _pmsm.isAttacking = false;
        _pmsm.playerLookDirection.canLook = true;
        _pmsm.canDoAction = true;
    }
}
