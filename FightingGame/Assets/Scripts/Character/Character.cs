using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IGameStateHolder<GameStateMatch>, IInputReceiver
{
    [SerializeField] private CharacterStateDescription _initialState = null;

    private ETeam _team;

    private CharacterStateMachine _stateMachine = null;
    private readonly List<IGameStateHolder<GameStateMatch>> _gameStateHolders = new();

    public void Initialize(
        ETeam team
        )
    {
        _team = team;
        _stateMachine = new CharacterStateMachine( transform );
        _stateMachine.SetState( _initialState );
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
        GameStateMatch game_state
        )
    {
        _gameStateHolders.ForEach( element => element.UpdateGameState( game_state ) );
    }

    public void RollbackToGameState(
        GameStateMatch game_state
        )
    {
        _gameStateHolders.ForEach( element => element.RollbackToGameState( game_state ) );
    }
}
