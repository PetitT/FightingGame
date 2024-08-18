using Cysharp.Threading.Tasks;
using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : NetworkBehaviour
{
    [SerializeField] private GameDatas _gameDatas;

    private readonly List<PlayerRef> _readyPlayersList = new();
    private PlayerRef _clientPlayer = PlayerRef.None;

    public readonly BufferedEvent OnGameStarted = new();


    private int _ticksForGameStart => _gameDatas.TicksForGameStart;

    public void SetReady()
    {
        RPC_SetReady();
    }

    [Rpc( RpcSources.All, RpcTargets.StateAuthority )]
    private void RPC_SetReady(
        RpcInfo info = default
        )
    {
        _readyPlayersList.Add( info.Source );

        if( !info.IsInvokeLocal )
        {
            _clientPlayer = info.Source;
        }

        if( _readyPlayersList.Count == ScenarioManager.Instance.ActiveScenario.ConnexionHandler.MaxPlayersCount )
        {
            RPC_StartGame();
        }
    }

    [Rpc( RpcSources.StateAuthority, RpcTargets.All )]
    private void RPC_StartGame()
    {
        StartGameAsync().Forget();
    }

    private async UniTask StartGameAsync()
    {
        if( HasStateAuthority )
        {
            Debug.Log( $"End of wait {DateTime.Now.Second} - {DateTime.Now.Millisecond}" );
            await UniTask.Delay( TimeSpan.FromSeconds( ScenarioManager.Instance.ActiveScenario.ConnexionHandler.GetPlayerRtt( _clientPlayer ) / 4 ) );
        }

        Debug.Log( $"End of wait {DateTime.Now.Second} - {DateTime.Now.Millisecond}" );
        OnGameStarted.Invoke();
    }
}
