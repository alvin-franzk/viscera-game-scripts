using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    EnemyBaseState currentState;

    void Start()
    {
        currentState = GetInitialState();
        currentState?.Enter();
    }

    void Update()
    {
        currentState?.UpdateLogic();
    }

    void LateUpdate()
    {
        currentState?.UpdatePhysics();
    }

    protected virtual EnemyBaseState GetInitialState()
    {
        return null;
    }

    public void ChangeState(EnemyBaseState newState)
    {
        currentState.Exit();

        currentState = newState;
        newState.Enter();
    }
}
