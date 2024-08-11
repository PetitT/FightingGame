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

    public int CurrentTick => Runner.Tick.Raw;

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
        CheckForGameStart();
        UpdateInputs();
    }

    private void CheckForGameStart()
    {
        if( StartTick == INVALID_TICK
            || _gameStarted
            )
        {
            return;
        }

        if( CurrentTick == StartTick )
        {
            _gameStarted = true;
            _inputReader.Initialize( _team );
            _inputManager.Initialize( GameManager.Instance.InputReceiverManager, GameManager.Instance.GameStateManager );
            Debug.Log( $"Tick {StartTick} attained, starting the game" );
        }
    }

    private void UpdateInputs()
    {
        _inputReader.OnFixedUpdateNetwork();
        _inputManager.OnFixedUpdateNetwork( CurrentTick );
    }

    private void GameManager_OnStartTickDecided(
        int tick
        )
    {
        Debug.Log( $"Received start tick: {tick}, current is {CurrentTick}" );
        StartTick = tick;
    }
}
