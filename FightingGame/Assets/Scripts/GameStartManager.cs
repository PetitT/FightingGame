using Fusion;
using UnityEngine;

public class GameStartManager : NetworkBehaviour
{

    [SerializeField] private GameDatas _gameDatas;

    public readonly BufferedEvent<int> OnStartTickDecided = new();

    [Networked, OnChangedRender( nameof( OnStartTickChanged ) )] private int _startTick { get; set; }
    private int _ticksForGameStart => _gameDatas.TicksForGameStart;

    public override void Spawned()
    {
        if( !HasStateAuthority
            && _startTick != 0
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
        _startTick = info.Tick.Raw + _ticksForGameStart;
    }

    private void OnStartTickChanged()
    {
        Debug.Log( $"Start tick changed to {_startTick}" );
        OnStartTickDecided.FireEvent( _startTick );
    }
}
