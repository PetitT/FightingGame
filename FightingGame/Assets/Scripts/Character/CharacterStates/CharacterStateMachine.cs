public class CharacterStateMachine : IGameStateHolder<GameStateBase>
{
    private CharacterStateBase _currentState;

    public void SetState(
        CharacterStateDescription description
        )
    {
        _currentState = description.GetCharacterState();
    }

    public void ProcessInputs(
        Inputs inputs
        )
    {
        _currentState.ProcessInput( inputs );
    }

    public void UpdateGameState(
        GameStateBase gameState
        )
    {

    }

    public void RollbackToGameState(
        GameStateBase gameState
        )
    {

    }
}
