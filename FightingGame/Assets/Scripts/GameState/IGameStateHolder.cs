public interface IGameStateHolder
{
    public void StoreGameState( GameState gameState );
    public void RollbackToGameState( GameState gameState );
}
