using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDying : EnemyBaseState
{
    private EnemyMovementStateMachine _emsm;

    public EnemyDying(EnemyMovementStateMachine stateMachine) : base(stateMachine) => _emsm = (EnemyMovementStateMachine)stateMachine;

    public override void Enter()
    {
        base.Enter();
        _emsm.animator.SetBool("Dies", true);
        SoundManager.instance.PlaySoundRandom(_emsm.deathSound, _emsm.transform, 0.6f);
        _emsm.StartCoroutine(_emsm.WaitBeforeDying());
    }

    public override void Exit()
    {
        base.Exit();

    }
}
