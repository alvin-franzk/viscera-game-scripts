using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : EnemyBaseState
{
    private EnemyMovementStateMachine _emsm;

    private GameObject _player;
    private PlayerMovementStateMachine _pmsm;

    public EnemyAttacking(EnemyMovementStateMachine stateMachine) : base(stateMachine)
    {
        // NOTE: Initialized _pmsm again to get rid of "_emsm._pmsm" call
        _player = GameObject.FindGameObjectWithTag("Player");
        _pmsm = _player.GetComponent<PlayerMovementStateMachine>();
        _emsm = (EnemyMovementStateMachine)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _emsm.isNearPlayer = Physics.CheckSphere(_emsm.transform.position, _emsm.attackRange, _emsm.whatIsPlayer);
        if (!_emsm._pmsm.playerStatus.isDead)
        {
            // Transition to Chasing State
            if (!_emsm.isNearPlayer)
            {
                _emsm.ChangeState(_emsm.chasingState);
            }
        }
        else
        {
            _emsm.ChangeState(_emsm.patrollingState);
        }

        // Transition to Dying State
        if (_emsm.enemyStatus.isDead)
        {
            _emsm.ChangeState(_emsm.dyingState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        // Make sure enemy doesn't move when attacking
        _emsm.agent.SetDestination(_emsm.transform.position);
        
        _emsm.transform.LookAt(_emsm.player.transform);

        if (!_emsm.isAttacking)
        {
            // 1. Play animation
            Lunge();
            _emsm.animator.SetTrigger("TriggerAnkleBite");
            // play sound
            SoundManager.instance.PlaySoundRandom(_emsm.attackSound, _emsm.transform, 0.2f);
            // 2. Check if attack animation hits player collider
             if (_emsm.attackCollider.IsCollidingWithPlayer)
             {
                HandleAttack();
             }
            
            _emsm.isAttacking = true;
            _emsm.StartCoroutine(ResetAttackCooldown());
            _emsm.ChangeState(_emsm.patrollingState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void HandleAttack()
    {
        if (_pmsm.isBlocking)
        {
            var reducedDamage = _emsm.attackDamage - (_emsm.attackDamage * _pmsm.blockReduction);
            var penaltyDamage = _emsm.attackDamage - (_emsm.attackDamage * (_pmsm.blockReduction - 0.35f));

            // Redirect damage to stamina and reduce by 50%
            if (_pmsm.playerStatus.currentStamina >= Mathf.Abs(_emsm.attackDamage))
            {
                _pmsm.animator.SetTrigger("BlockedHit");
                _pmsm.playerStatus.AdjustCurrentStamina(reducedDamage);
            }
            // Penalize player if blocking with insufficient stamina
            else
            {
                _pmsm.ChangeState(_pmsm.hurtState);
                _pmsm.playerStatus.AdjustCurrentHealth(penaltyDamage);
                _pmsm.playerStatus.AdjustCurrentStamina(penaltyDamage);
            }
        }
        
        if (_pmsm.playerStatus.isInvulnerable)
        {
            // Damage is negated when dodging
            _pmsm.playerStatus.AdjustCurrentHealth(0);
        }
        
        // Successful attack hit
        if (!_pmsm.isBlocking && !_pmsm.playerStatus.isInvulnerable)
        {
            _pmsm.ChangeState(_pmsm.hurtState);
            _pmsm.playerStatus.AdjustCurrentHealth(_emsm.attackDamage);
        }
        
    }

    private void Lunge()
    {
        Vector3 direction = (_emsm._pmsm.transform.position - _emsm.transform.position).normalized;

        _emsm.rb.AddForce(direction * _emsm.lungeStrength, ForceMode.Impulse);
    }

    private IEnumerator ResetAttackCooldown()
    {
        while(_emsm.isAttacking)
        {
            _emsm.rb.velocity = Vector3.zero;
            yield return new WaitForSeconds(_emsm.attackCooldown);
            _emsm.isAttacking = false;
        }
        
    }
}
