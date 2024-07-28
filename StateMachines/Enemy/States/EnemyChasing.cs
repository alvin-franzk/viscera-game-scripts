using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasing : EnemyBaseState
{
    private EnemyMovementStateMachine _emsm;
    private float timer = 0f;
    public EnemyChasing(EnemyMovementStateMachine stateMachine) : base(stateMachine) => _emsm = (EnemyMovementStateMachine)stateMachine;

    public override void Enter()
    {
        base.Enter();
        _emsm.animator.SetBool("Run", true);
        SoundManager.instance.PlaySoundRandom(_emsm.alertSound, _emsm.transform, 0.1f);
        SoundManager.instance.PlaySoundRandom(_emsm.runSound, _emsm.transform, 0.1f);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_emsm._pmsm.playerStatus.isDead)
        {
            _emsm.canSeePlayer = Physics.CheckSphere(_emsm.transform.position, _emsm.sightRange, _emsm.whatIsPlayer);
            _emsm.isNearPlayer = Physics.CheckSphere(_emsm.transform.position, _emsm.attackRange, _emsm.whatIsPlayer);

            // Transition to Attacking State
            if (_emsm.isNearPlayer)
            {
                _emsm.ChangeState(_emsm.attackingState);
            }

            // Transition to Patrolling State
            if (!_emsm.canSeePlayer)
            {
                _emsm.ChangeState(_emsm.patrollingState);
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

        // Make Enemy chase player
        _emsm.agent.SetDestination(_emsm.player.transform.position);

        // Play audio
        PlayAudio();
    }

    public override void Exit()
    {
        base.Exit();
        _emsm.animator.SetBool("Run", false);
    }

    private void PlayAudio()
    {
        if (timer < 4f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SoundManager.instance.PlaySoundRandom(_emsm.runSound, _emsm.transform, 0.1f);
            timer = 0f;
        }
    }
}
