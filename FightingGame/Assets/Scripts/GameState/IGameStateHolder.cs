public interface IGameStateHolder<T> where T : GameStateBase
{
    /// <summary>
    /// Write all the game state into the game_state parameter
    /// </summary>
    /// <param name="game_state"></param>
    public void UpdateGameState( T game_state );
    public void RollbackToGameState( T game_state );
}
