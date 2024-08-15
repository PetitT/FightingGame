using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class CharacterInputHandler : IInputReceiver, IGameStateHolder<GameStateMatch>
{
    private ETeam _team = ETeam.None;

    private UnityEvent<Command> _onCommand = new();
    public UnityEvent<Command> OnCommand => _onCommand;

    //private readonly List<InputCommand> _commandBuffer = new(); Add this when implementing attack

    public CharacterInputHandler(
        ETeam team
        )
    {
        _team = team;
    }

    public void ReceiveInputs(
        IReadOnlyList<Inputs> inputs
        )
    {
        Inputs local_input = inputs.FirstOrDefault( input => input.Team == _team );

        AddCommandToBuffer( local_input );
        ApplyMoveCommand( local_input );
    }

    private void AddCommandToBuffer(
        Inputs local_input
        )
    {
        //_commandBuffer.AddRange( inputs.Where( input => input.Team == _team ).Select( input => new InputCommand( input ) ) );
    }

    private void ApplyMoveCommand(
        Inputs local_input
        )
    {
        OnCommand.Invoke( new Command( local_input.GetMoveDirection() ) );
    }

    public void RollbackToGameState(
        GameStateMatch game_state
        )
    {
        //copy command buffer
    }

    public void UpdateGameState(
        GameStateMatch game_state
        )
    {
        // restore command buffer
    }
}

public readonly struct Command
{
    //Add any action here like attack or dash or whatever
    public EMoveDirection MoveDirection { get; }

    public Command(
        EMoveDirection move_direction
        )
    {
        MoveDirection = move_direction;
    }
}