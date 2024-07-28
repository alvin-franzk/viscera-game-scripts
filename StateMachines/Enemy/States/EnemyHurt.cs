using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyHurt : EnemyBaseState
{
    private EnemyMovementStateMachine _emsm;
    public int noOfAttacks = 0;
    public EnemyHurt(EnemyMovementStateMachine stateMachine) : base(stateMachine) => _emsm = (EnemyMovementStateMachine)stateMachine;

    public override void Enter()
    {
        base.Enter();
        _emsm.isHurt = true;
        noOfAttacks++;
        HandleDamage(); // normal knockback, if crit, super knockback
        SoundManager.instance.PlaySoundRandom(_emsm.hurtBladeSound, _emsm.transform, 0.5f);
        SoundManager.instance.PlaySoundRandom(_emsm.hurtSound, _emsm.transform, 0.5f);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_emsm.enemyStatus.isDead)
        {
            _emsm.ChangeState(_emsm.dyingState);
        }
        else
        {
            _emsm.ChangeState(_emsm.patrollingState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

    }
    public override void Exit()
    {
        base.Exit();

        _emsm.StartCoroutine(_emsm.ResetStunDuration());
    }

    private void HandleDamage()
    {
        if (_emsm._pmsm.CritRoll())
        {
            var critDamage = _emsm._pmsm.attackRoll() * _emsm._pmsm.critModifier;
            _emsm.enemyStatus.AdjustCurrentHealth(critDamage);
            Knockback(_emsm._pmsm.critChance * 50);
            _emsm.accumulatedScore += (CalculateScore(noOfAttacks, critDamage) / 2);
        }
        else
        {
            var damage = _emsm._pmsm.attackRoll();
            _emsm.enemyStatus.AdjustCurrentHealth(damage);
            Knockback(0);
            _emsm.accumulatedScore += (CalculateScore(noOfAttacks, damage) / 2);
        }
    }

    private void Knockback(float modifier)
    {
        Vector3 direction = (_emsm.transform.position - _emsm._pmsm.transform.position).normalized;

        _emsm.rb.AddForce(direction * (_emsm._pmsm.knockbackStrength + modifier), ForceMode.Impulse);
    }

    private int CalculateScore(int noOfAttacks, float damage)
    {
        int score = ((int)_emsm.enemyStatus.maxHealth - noOfAttacks) + Mathf.Abs((int)damage);

        return score;
    }
}
