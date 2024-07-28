using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerBaseState currentState;
    
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

    protected virtual PlayerBaseState GetInitialState()
    {
        return null;
    }

    public void ChangeState(PlayerBaseState newState)
    {
        currentState.Exit();

        currentState = newState;
        newState.Enter();
    }
}
