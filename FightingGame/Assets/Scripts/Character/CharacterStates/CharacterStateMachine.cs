using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : IGameStateHolder<GameStateBase>
{
    private CharacterStateBase _currentState;

    private CharacterStateMachine() { }

    public CharacterStateMachine(
        CharacterStateBase initial_state
        )
    {
        _currentState = initial_state;
    }

    public void ProcessInputs(
        Inputs inputs
        )
    {
        _currentState.ProcessInput( inputs );
    }

    public void UpdateGameState( GameStateBase gameState )
    {
        throw new System.NotImplementedException();
    }

    public void RollbackToGameState( GameStateBase gameState )
    {
        throw new System.NotImplementedException();
    }

}
