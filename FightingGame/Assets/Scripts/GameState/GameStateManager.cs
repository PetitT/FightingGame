using System;
using System.Collections.Generic;

public class GameStateManager<T> where T : GameStateBase
{
    private readonly List<IGameStateHolder<T>> GameStateHolders = new();

    public void RegisterGameStateHolder(
        IGameStateHolder<T> gameStateHolder
        )
    {
        GameStateHolders.Add( gameStateHolder );
    }

    public T GetGameState()
    {
        T new_object = (T)Activator.CreateInstance( typeof( T ) );

        foreach( IGameStateHolder<T> game_state_holder in GameStateHolders )
        {
            game_state_holder.UpdateGameState( new_object );
        }

        return new_object;
    }

    public void RestoreGameState(
        T game_state
        )
    {
        foreach( IGameStateHolder<T> game_state_holder in GameStateHolders )
        {
            game_state_holder.RollbackToGameState( game_state );
        }
    }
}
