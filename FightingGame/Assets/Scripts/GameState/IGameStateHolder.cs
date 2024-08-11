public interface IGameStateHolder
{
    public void UpdateGameState( GameState gameState );
    public void RollbackToGameState( GameState gameState );
}
