using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IGameStateHolder<GameStateMatch>, IInputReceiver
{
    [SerializeField] private CharacterStateDescription _initialState = null;

    private ETeam _team;

    private CharacterStateMachine _stateMachine = null;
    private CharacterInputHandler _inputHandler = null;

    private readonly List<IGameStateHolder<GameStateMatch>> _gameStateHolders = new();

    public void Initialize(
        ETeam team
        )
    {
        _team = team;
        _stateMachine = new CharacterStateMachine( transform );
        _stateMachine.SetState( _initialState );
        _gameStateHolders.Add( _stateMachine );

        _inputHandler = new CharacterInputHandler( _team );

        _inputHandler.OnCommand.AddListener( InputHandler_OnCommand );
    }

    private void InputHandler_OnCommand( 
        Command command 
        )
    {
        _stateMachine.ProcessCommand( command );
    }

    public void ReceiveInputs(
        IReadOnlyList<Inputs> inputs
        )
    {
        _inputHandler.ReceiveInputs( inputs );
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

    private void OnDestroy()
    {
        _inputHandler.OnCommand.RemoveListener( InputHandler_OnCommand );
    }
}
