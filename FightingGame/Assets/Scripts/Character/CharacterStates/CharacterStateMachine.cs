using UnityEngine;

public class CharacterStateMachine : IGameStateHolder<GameStateMatch>
{
    private Transform _origin;
    private CharacterStateBase _currentState;

    private CharacterStateDescription _currentDescription;

    public CharacterStateMachine(
        Transform origin
        )
    {
        _origin = origin;
    }

    public void SetState(
        CharacterStateDescription description,
        int tick = 0
        )
    {
        _currentState = description.GetCharacterState( _origin, tick );
        _currentDescription = description;
    }

    public void ProcessCommand(
        Command command
        )
    {
        _currentState.ProcessCommand( command );
    }

    public void UpdateGameState(
        GameStateMatch game_state
        )
    {
        game_state._stateMachineData = new GameStateMatch.StateMachineData
        {
            Description = _currentDescription,
            Tick = _currentState.CurrentTick
        };
    }

    public void RollbackToGameState(
        GameStateMatch game_state
        )
    {
        SetState( game_state._stateMachineData.Description, game_state._stateMachineData.Tick );
    }
}
