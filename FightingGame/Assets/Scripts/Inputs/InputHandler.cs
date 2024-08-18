using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Do I really want a reference to game state here ?
public class InputHandler<T> where T : GameStateBase
{
    private readonly Dictionary<int, InputInfos> _inputInfos = new();
    private readonly Dictionary<int, T> _predictedGameStates = new();

    private GameStateManager<T> _gameStateManager;
    private InputReceiverManager _inputReceiverManager;

    private Inputs _lastReceivedOpponentInput = new();

    private int _expectedPlayerCount = 0;
    private int _inputDelay = 0;
    private int _tickToProcess = 0;
    private bool _enableLogs = true; //Add a way to easily modify this

    private bool IsInPrediction => _inputInfos.Count > 0 && _inputInfos.First().Key < _tickToProcess;

    private InputHandler() { }

    public InputHandler(
        GameStateManager<T> game_state_manager,
        InputReceiverManager input_receiver_manager,
        int expected_player_count,
        int input_delay,
        ETeam local_team
        )
    {
        _gameStateManager = game_state_manager;
        _inputReceiverManager = input_receiver_manager;
        _expectedPlayerCount = expected_player_count;
        _inputDelay = input_delay;
        _lastReceivedOpponentInput = new Inputs( local_team.GetOppositeTeam() );
    }

    public void OnFixedUpdateNetwork(
        int tick
        )
    {
        _tickToProcess = tick;
        Log( $"Applying tick {_tickToProcess}" );
        RestoreToRealGameState();
        ApplyCurrentTick();
    }

    private void StoreGameState(
        int tick
        )
    {
        if( _predictedGameStates.ContainsKey( tick ) )
        {
            Log( $"Overwriting game state for tick {tick}." );
            _predictedGameStates.Remove( tick );
        }

        _predictedGameStates.Add( tick, _gameStateManager.GetGameState() );
    }

    private void ApplyCurrentTick()
    {
        if( !_inputInfos.ContainsKey( _tickToProcess ) )
        {
            Log( $"No input infos found for tick {_tickToProcess}." );
            return;
        }

        InputInfos current_infos = _inputInfos[ _tickToProcess ];

        if( !current_infos.HasInputsFromAllPlayers() )
        {
            current_infos.AddPredictedInput( _lastReceivedOpponentInput );
            ApplyInputs( current_infos.PredictedInputs, _tickToProcess );
            Log( "Predicted tick" );
        }
        else
        {
            ApplyInputs( current_infos.RealInputs, _tickToProcess );
            ValidateTick( _tickToProcess );
            Log( $"Applied real tick" );
        }
    }

    private void RestoreToRealGameState()
    {
        if( !IsInPrediction )
        {
            return;
        }

        int first_tick = _inputInfos.First().Key;

        for( int target_tick = first_tick; target_tick < _tickToProcess; target_tick++ )
        {
            InputInfos input_infos = _inputInfos[ target_tick ];

            if( !input_infos.HasInputsFromAllPlayers() )
            {
                return;
            }

            if( input_infos.RealAndPredictedInputsMatch() )
            {
                Log( $"Tick {target_tick} was predicted correctly" );
                ValidateTick( target_tick );

                continue;
            }

            RollbackToTick( target_tick );
            ReplayAllTicksFrom( target_tick );

            return;
        }
    }

    private void ApplyInputs(
        IReadOnlyList<Inputs> inputs,
        int tick
        )
    {
        StoreGameState( tick );
        _inputReceiverManager.ApplyInputs( inputs );
    }

    private void RollbackToTick(
        int tick
        )
    {
        if( !_predictedGameStates.ContainsKey( tick ) )
        {
            Log( $"No game state found for tick {tick} in predicted game states." );
            return;
        }

        Log( $"Rolling back to tick {tick}" );
        _gameStateManager.RestoreGameState( _predictedGameStates[ tick ] ); // might need to restore to previous tick
        _predictedGameStates.Clear();
    }

    private void ReplayAllTicksFrom(
        int initial_tick
        )
    {
        for( int current_tick = initial_tick; current_tick < _tickToProcess; current_tick++ )
        {
            if( !_inputInfos.ContainsKey( current_tick ) )
            {
                Log( $"No input infos found for tick {current_tick}." );
                break;
            }

            InputInfos current_inputs = _inputInfos[ current_tick ];

            if( current_inputs.HasInputsFromAllPlayers() )
            {
                Log( $"Replaying tick {current_tick} with real input" );
                ApplyInputs( current_inputs.RealInputs, current_tick );
            }
            else
            {
                Log( $"Replaying tick {current_tick} with predicted inputs" );
                current_inputs.AddPredictedInput( _lastReceivedOpponentInput );
                ApplyInputs( current_inputs.PredictedInputs, current_tick );
            }
        }
    }

    private void ValidateTick(
        int tick
        )
    {
        _inputInfos.Remove( tick );
        _predictedGameStates.Remove( tick - 1 );
    }

    public void ReceiveInput(
        InputReceivedArgs input_received_args
        )
    {
        int tick_to_play = input_received_args.Tick + _inputDelay;
        Log( $"Receive input at tick {input_received_args.Tick} to be played at tick {tick_to_play} - {( input_received_args.IsLocalInput ? "Local" : "Remote" )}" );
        AddRealInputs( input_received_args.Inputs, tick_to_play );

        if( input_received_args.IsLocalInput )
        {
            AddPredictedInputs( input_received_args.Inputs, tick_to_play );
        }
        else
        {
            _lastReceivedOpponentInput = input_received_args.Inputs;
        }
    }

    private void AddRealInputs(
        Inputs input,
        int tick
        )
    {
        if( !_inputInfos.ContainsKey( tick ) )
        {
            _inputInfos.Add( tick, new InputInfos( _expectedPlayerCount ) );
        }

        _inputInfos[ tick ].AddRealInput( input );
    }

    private void AddPredictedInputs(
        Inputs input,
        int tick
        )
    {
        if( !_inputInfos.ContainsKey( tick ) )
        {
            _inputInfos.Add( tick, new InputInfos( _expectedPlayerCount ) );
        }

        _inputInfos[ tick ].AddPredictedInput( input );
    }

    private void Log( string message )
    {
        if( _enableLogs )
        {
            Debug.Log( message );
        }
    }
}
