using Cysharp.Threading.Tasks;
using Fusion;
using System;
using UnityEngine;

public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader = null;
    [SerializeField] private InputManager _inputManager = null;

    private bool _gameStarted = false;
    private int _currentTick = 0;

    [Networked] public ETeam Team { get; private set; }

    public override void Spawned()
    {
        gameObject.name = $"NetworkPlayerController_{Object.InputAuthority}";

        if( !HasInputAuthority )
        {
            return;
        }

        GameManager.Instance.GameStartManager.OnGameStarted.AddListener( GameManager_OnGameStarted );
        GameManager.Instance.GameStartManager.SetReady();
    }

    public override void Despawned(
        NetworkRunner runner,
        bool has_state
        )
    {
        GameManager.Instance.GameStartManager.OnGameStarted.RemoveListener( GameManager_OnGameStarted );
    }

    public void Initialize(
        ETeam team
        )
    {
        Team = team;
    }

    private async UniTaskVoid UpdateLoopAsync()
    {
        while( true )
        {
            UpdateInputs();
            await UniTask.Delay( TimeSpan.FromSeconds( Runner.DeltaTime ) );
        }
    }

    private void UpdateInputs()
    {
        _inputReader.OnFixedUpdateNetwork( _currentTick );
        _inputManager.OnFixedUpdateNetwork( _currentTick );
        _currentTick++;
    }

    private void GameManager_OnGameStarted()
    {
        Debug.Log( $"Game begin" );

        _inputReader.Initialize( Team );
        _inputManager.Initialize( GameManager.Instance.InputReceiverManager, GameManager.Instance.GameStateManager, Team );
        GameManager.Instance.CharacterManager.SpawnAllCharacters();
        UpdateLoopAsync().Forget();
    }
}
