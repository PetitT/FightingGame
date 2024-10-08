using UnityEngine;

public class CharacterStateMachine : IGameStateHolder<GameStateMatch>
{
    private CharacterStateData _characterStateData = null;
    private Transform _origin = null;
    private CharacterStateBase _currentState = null;
    private CharacterStateDescription _currentDescription = null;

    public CharacterStateMachine(
        CharacterStateData characterStateData,
        Transform origin
        )
    {
        _characterStateData = characterStateData;
        _origin = origin;
    }

    public void SetState(
        CharacterStateDescription description,
        int tick = 0
        )
    {
        if( _currentState != null )
        {
            _currentState.Clear();
        }

        _currentState = description.GetCharacterState( _origin, tick );
        _currentDescription = description;
    }

    public void ProcessCommand(
        Command command
        )
    {
        if( TryGetNextState( command, out CharacterStateDescription next_state ) )
        {
            SetState( next_state );
        }

        _currentState.ProcessCommand( command );
    }

    public void UpdateGameState(
        GameStateMatch game_state
        )
    {
        game_state._stateMachineData = new GameStateMatch.StateMachineData
        {
            Description = _currentDescription,
            Tick = _currentState.CurrentTick,
            Position = _origin.transform.position
        };
    }

    public void RollbackToGameState(
        GameStateMatch game_state
        )
    {
        SetState( game_state._stateMachineData.Description, game_state._stateMachineData.Tick );
        _origin.transform.position = game_state._stateMachineData.Position;
    }

    //This should be a behaviour tree
    public bool TryGetNextState(
        Command command,
        out CharacterStateDescription next_state
        )
    {
        if( _currentState == null )
        {
            next_state = _characterStateData.IdleState;

            return true;
        }

        if( _currentState is CharacterStateIdle
            && command.MoveDirection != EMoveDirection.None
            )
        {
            next_state = _characterStateData.WalkState;

            return true;
        }

        if( _currentState is CharacterStateMove
            && command.MoveDirection == EMoveDirection.None
            )
        {
            next_state = _characterStateData.IdleState;

            return true;
        }

        next_state = null;

        return false;
    }
}