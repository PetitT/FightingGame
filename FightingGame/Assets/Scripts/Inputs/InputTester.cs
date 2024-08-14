using System.Collections.Generic;
using UnityEngine;

public class InputTester : MonoBehaviour, IGameStateHolder<TestGameState>, IInputReceiver
{
    [SerializeField] private Inputs _p1Input = default;
    [SerializeField] private Inputs _p2Input = default;
    [SerializeField, Fusion.ReadOnly] private int _nextPlayedTick = 0;
    [SerializeField, Fusion.ReadOnly] private int _targetTickForP2 = 0;

    [SerializeField, Fusion.ReadOnly] private int TotalAttacks = 0;
    private int InputDelay = 2;

    private GameStateManager<TestGameState> _gameStateManager = new();
    private InputReceiverManager _inputReceiverManager = new();
    private InputHandler<TestGameState> _inputHandler = null;

    public bool HasSentP1InputThisFrame { get; set; }

    private void Start()
    {
        _inputHandler = new InputHandler<TestGameState>( _gameStateManager, _inputReceiverManager, expected_player_count: 2, InputDelay, ETeam.TeamOne );
        _gameStateManager.RegisterGameStateHolder( this );
        _inputReceiverManager.RegisterInputReceiver( this );
    }

    public void SendP1Input()
    {
        if( HasSentP1InputThisFrame )
        {
            return;
        }

        _inputHandler.ReceiveInput( new InputReceivedArgs( _nextPlayedTick, _p1Input, true ) );

        HasSentP1InputThisFrame = true;
    }

    public void SendP2Input()
    {
        _inputHandler.ReceiveInput( new InputReceivedArgs( _targetTickForP2, _p2Input, false ) );
        _targetTickForP2++;
    }

    public void UpdateInputs()
    {
        _inputHandler.OnFixedUpdateNetwork( _nextPlayedTick );
        _nextPlayedTick++;
        HasSentP1InputThisFrame = false;
    }

    public void ReceiveInputs(
        IReadOnlyList<Inputs> inputs
        )
    {
        foreach( var input in inputs )
        {
            if( input.IsAttacking )
            {
                TotalAttacks++;
            }
        }
    }

    public void UpdateGameState(
        TestGameState game_state
        )
    {
        game_state.TotalAttacks = TotalAttacks;
    }

    public void RollbackToGameState(
        TestGameState game_state
        )
    {
       TotalAttacks = game_state.TotalAttacks;
    }
}
