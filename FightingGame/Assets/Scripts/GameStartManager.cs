using Fusion;
using UnityEngine;

public class GameStartManager : NetworkBehaviour
{
    private const int TICKS_FOR_GAME_START = 100;

    [Networked, OnChangedRender( nameof( OnStartTickChanged ) )] private int StartTick { get; set; }

    public BufferedEvent<int> OnStartTickDecided = new();

    public override void Spawned()
    {
        if( !HasStateAuthority
            && StartTick != 0
            )
        {
            OnStartTickChanged();
        }
    }

    public void StartGame()
    {
        RPC_StartGame();
    }

    [Rpc( RpcSources.StateAuthority, RpcTargets.StateAuthority )]
    private void RPC_StartGame(
        RpcInfo info = default
        )
    {
        StartTick = info.Tick.Raw + TICKS_FOR_GAME_START;
    }

    private void OnStartTickChanged()
    {
        Debug.Log( $"Start tick changed to {StartTick}" );
        OnStartTickDecided.FireEvent( StartTick );
    }
}
