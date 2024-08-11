using Fusion;
using System.Collections.Generic;

public class GameStateManager : NetworkBehaviour
{
    private readonly List<IGameStateHolder> GameStateHolders = new();

    public void RegisterGameStateHolder(
        IGameStateHolder gameStateHolder
        )
    {
        GameStateHolders.Add( gameStateHolder );
    }

    public GameState GetGameState()
    {
        GameState game_state = new();

        foreach( IGameStateHolder game_state_holder in GameStateHolders )
        {
            game_state_holder.StoreGameState( game_state );
        }

        return game_state;
    }

    public void RestoreGameState(
        GameState game_state
        )
    {
        foreach( IGameStateHolder game_state_holder in GameStateHolders )
        {
            game_state_holder.RollbackToGameState( game_state );
        }
    }
}
