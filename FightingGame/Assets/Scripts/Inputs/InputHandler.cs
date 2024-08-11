using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputHandler
{
    private const int INPUT_DELAY = 3;

    private readonly Dictionary<int, InputInfos> _inputInfos = new();
    private readonly Dictionary<int, GameState> _predictedGameStates = new();

    private Inputs LastReceivedOpponentInput = new(); // FIXME : The first input here is always on team1

    private int CurrentRealTick = 0;
    private int CurrentTickToPlay => CurrentRealTick - INPUT_DELAY;

    private bool IsInPrediction => _inputInfos.Count > 0 && _inputInfos.First().Key < CurrentTickToPlay;

    public void OnFixedUpdateNetwork(
        int tick
        )
    {
        CurrentRealTick = tick;
        RestoreToRealGameState();
        ApplyCurrentTick();
    }

    private void StoreGameState(
        int tick
        )
    {
        if( _predictedGameStates.ContainsKey( tick ) )
        {
            Debug.LogWarning( $"Overwriting game state for tick {tick}." );
            _predictedGameStates.Remove( tick );
        }

        _predictedGameStates.Add( tick, GameManager.Instance.GameStateManager.GetGameState() );
    }
    private void ApplyCurrentTick()
    {
        if( IsInPrediction )
        {
            return;
        }

        if( !_inputInfos.ContainsKey( CurrentTickToPlay ) )
        {
            Debug.LogError( $"No input infos found for tick {CurrentTickToPlay}." );

            return;
        }

        InputInfos current_infos = _inputInfos[ CurrentTickToPlay ];

        if( !current_infos.HasInputsFromAllPlayers() )
        {
            current_infos.AddPredictedInput( LastReceivedOpponentInput );
            ApplyInputs( current_infos.PredictedInputs, CurrentTickToPlay );
        }
        else
        {
            ApplyInputs( current_infos.RealInputs, CurrentTickToPlay );
            ValidateTick( CurrentTickToPlay );
        }
    }

    private void RestoreToRealGameState()
    {
        if( !IsInPrediction )
        {
            return;
        }

        int first_tick = _inputInfos.First().Key;

        for( int target_tick = first_tick; target_tick < CurrentTickToPlay; target_tick++ )
        {
            InputInfos input_infos = _inputInfos[ target_tick ];

            if( !input_infos.HasInputsFromAllPlayers() )
            {
                return;
            }

            if( input_infos.RealAndPredictedInputsMatch() )
            {
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
        // Apply the inputs
    }

    private void RollbackToTick(
        int tick
        )
    {
        if( !_predictedGameStates.ContainsKey( tick ) )
        {
            Debug.LogError( $"No game state found for tick {tick} in predicted game states." );

            return;
        }

        GameManager.Instance.GameStateManager.RestoreGameState( _predictedGameStates[ tick ] ); // might need to restore to previous tick
        _predictedGameStates.Clear();
    }

    private void ReplayAllTicksFrom(
        int initial_tick
        )
    {
        for( int current_tick = initial_tick; current_tick <= CurrentTickToPlay; current_tick++ )
        {
            InputInfos current_inputs = _inputInfos[ current_tick ];

            if( current_inputs.HasInputsFromAllPlayers() )
            {
                ApplyInputs( current_inputs.RealInputs, current_tick );
            }
            else
            {
                current_inputs.AddPredictedInput( LastReceivedOpponentInput );
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
        AddRealInputs( input_received_args.Inputs, input_received_args.Tick );

        if( !input_received_args.IsLocalInput )
        {
            LastReceivedOpponentInput = input_received_args.Inputs;
        }
    }

    private void AddRealInputs(
        Inputs input,
        int tick
        )
    {
        if( !_inputInfos.ContainsKey( tick ) )
        {
            _inputInfos.Add( tick, new InputInfos() );
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
            _inputInfos.Add( tick, new InputInfos() );
        }

        _inputInfos[ tick ].AddPredictedInput( input );
    }
}