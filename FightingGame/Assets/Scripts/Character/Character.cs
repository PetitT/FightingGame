using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IGameStateHolder<GameStateBase>, IInputReceiver
{
    [SerializeField] private CharacterStateBase _initialState;

    private ETeam _team;

    private CharacterStateMachine _stateMachine = null;
    private readonly List<IGameStateHolder<GameStateBase>> _gameStateHolders = new();

    public void Initialize(
        ETeam team
        )
    {
        _team = team;
        _stateMachine = new CharacterStateMachine( _initialState );
        _gameStateHolders.Add( _stateMachine );
    }

    public void ReceiveInputs(
        IReadOnlyList<Inputs> inputs
        )
    {
        foreach( Inputs input in inputs )
        {
            if( input.Team == _team )
            {
                _stateMachine.ProcessInputs( input );
            }
        }
    }

    public void UpdateGameState(
        GameStateBase gameState
        )
    {
        foreach( IGameStateHolder<GameStateBase> game_state_holder in _gameStateHolders )
        {
            game_state_holder.UpdateGameState( gameState );
        }
    }

    public void RollbackToGameState(
        GameStateBase gameState
        )
    {
        foreach( IGameStateHolder<GameStateBase> game_state_holder in _gameStateHolders )
        {
            game_state_holder.RollbackToGameState( gameState );
        }
    }
}
