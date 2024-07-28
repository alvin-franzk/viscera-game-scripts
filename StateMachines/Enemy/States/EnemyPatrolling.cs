using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolling : EnemyBaseState
{
    private EnemyMovementStateMachine _emsm;
    /*
    private Vector3 _walkPoint;
    private float _walkPointRange;
    private bool _walkPointSet;
    */

    private float range;

    public EnemyPatrolling(EnemyMovementStateMachine stateMachine) : base(stateMachine) => _emsm = (EnemyMovementStateMachine)stateMachine;

    public override void Enter()
    {
        base.Enter();
        _emsm.animator.SetBool("Sleeps", true);


    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // Checks if player is alive for the States
        if (!_emsm._pmsm.playerStatus.isDead)
        {
            _emsm.canSeePlayer = Physics.CheckSphere(_emsm.transform.position, _emsm.sightRange, _emsm.whatIsPlayer);
            _emsm.isNearPlayer = Physics.CheckSphere(_emsm.transform.position, _emsm.attackRange, _emsm.whatIsPlayer);

            // Transition to Chasing State
            if (_emsm.canSeePlayer && !_emsm.isNearPlayer)
            {
                _emsm.ChangeState(_emsm.chasingState);
            }

            // Transition to Attacking State
            if (_emsm.isNearPlayer)
            {
                _emsm.ChangeState(_emsm.attackingState);
            }
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

        // OLD PATROL CODE
        /*
        // Set walk points for patrol range
        if (!_walkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            _emsm.agent.SetDestination(_walkPoint);
        } 

        Vector3 distanceToWalkPoint = _emsm.transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            _walkPointSet = false;
        }
        */
        if (_emsm.agent.remainingDistance <= _emsm.agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(_emsm.transform.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                _emsm.agent.SetDestination(point);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _emsm.animator.SetBool("Sleeps", false);
        _emsm.animator.SetTrigger("TriggerAlert");
    }

    /*
    private void SearchWalkPoint()
    {
        // Calculate random range
        float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(_emsm.transform.position.x + randomX, _emsm.transform.position.y, _emsm.transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, _emsm.transform.up, 2f, _emsm.whatIsGround))
        {
            _walkPointSet = true;
        }
    }
    */

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
