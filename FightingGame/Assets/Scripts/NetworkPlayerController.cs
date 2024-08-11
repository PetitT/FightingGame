using Cysharp.Threading.Tasks.Triggers;
using Fusion;
using UnityEngine;

public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader = null;
    [SerializeField] private InputManager _inputManager = null;

    private const int INVALID_TICK = -1;
    private int StartTick = INVALID_TICK;

    private bool _gameStarted = false;
    private ETeam _team = ETeam.TeamOne;

    public override void Spawned()
    {
        gameObject.name = $"NetworkPlayerController_{Object.InputAuthority}";

        if( !HasInputAuthority )
        {
            return;
        }

        GameManager.Instance.GameStartManager.OnStartTickDecided.SetEventCallback( GameManager_OnStartTickDecided );
    }

    public override void Despawned(
        NetworkRunner runner,
        bool has_state
        )
    {
        GameManager.Instance.GameStartManager.OnStartTickDecided.ClearEventCallback( GameManager_OnStartTickDecided );
    }

    public void Initialize(
        ETeam team
        )
    {
        _team = team;
    }

    public override void FixedUpdateNetwork()
    {
        if( StartTick == INVALID_TICK
            || _gameStarted
            )
        {
            return;
        }

        if( Runner.Tick.Raw == StartTick )
        {
            _gameStarted = true;
            _inputReader.Initialize( _team );
            _inputManager.Initialize();
            Debug.Log( $"Tick {StartTick} attained, starting the game" );
        }
    }

    private void GameManager_OnStartTickDecided(
        int tick
        )
    {
        Debug.Log( $"Received start tick: {tick}, current is {Runner.Tick.Raw}" );
        StartTick = tick;
    }
}
