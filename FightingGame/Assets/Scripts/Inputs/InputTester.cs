using System.Collections.Generic;
using UnityEngine;

public class InputTester : MonoBehaviour, IGameStateHolder, IInputReceiver
{
    [SerializeField] private Inputs _p1Input = default;
    [SerializeField] private Inputs _p2Input = default;
    [SerializeField, Fusion.ReadOnly] private int _targetTickForP2 = 0;

    [SerializeField, Fusion.ReadOnly] private int _nextPlayedTick = 0;

    [SerializeField] private int InputDelay = 0;

    private GameStateManager _gameStateManager = new();
    private InputReceiverManager _inputReceiverManager = new();

    private InputHandler _inputHandler = null;

    [Fusion.ReadOnly] public bool HasSentP1InputThisFrame = false;

    private void Start()
    {
        _inputHandler = new InputHandler( _gameStateManager, _inputReceiverManager, expected_player_count: 2, InputDelay );
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

    }

    public void UpdateGameState(
        GameState gameState
        )
    {

    }

    public void RollbackToGameState(
        GameState gameState
        )
    {

    }
}
