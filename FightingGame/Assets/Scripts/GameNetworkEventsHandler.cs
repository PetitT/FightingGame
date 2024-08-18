using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using System.Linq;
using UnityEngine;

public class GameNetworkEventsHandler : MonoBehaviour
{
    [SerializeField] private GameManager _gameManagerPrefab = null;
    [SerializeField] private NetworkPlayerController _networkPlayerControllerPrefab = null;

    int _playerCount = 0;
    private bool _gameStarted = false;
    private GameManager _gameManager = null;

    private void Awake()
    {
        ScenarioManager.Instance.ActiveScenario.NetworkEventsListener.OnPlayerJoinedEvent.AddListener( ConnexionHandler_OnPlayerJoined );
        ScenarioManager.Instance.ActiveScenario.NetworkEventsListener.OnPlayerLeftEvent.AddListener( ConnexionHandler_OnPlayerLeft );
        ScenarioManager.Instance.ActiveScenario.NetworkEventsListener.OnDisconnectedFromServerEvent.AddListener( ConnexionHandler_OnDisconnectedFromServer );
    }

    private void OnDestroy()
    {
        if( ScenarioManager.HasInstance )
        {
            ScenarioManager.Instance.ActiveScenario.NetworkEventsListener.OnPlayerJoinedEvent.RemoveListener( ConnexionHandler_OnPlayerJoined );
            ScenarioManager.Instance.ActiveScenario.NetworkEventsListener.OnPlayerLeftEvent.RemoveListener( ConnexionHandler_OnPlayerLeft );
            ScenarioManager.Instance.ActiveScenario.NetworkEventsListener.OnDisconnectedFromServerEvent.RemoveListener( ConnexionHandler_OnDisconnectedFromServer );
        }
    }

    private void ConnexionHandler_OnPlayerJoined(
        NetworkRunner runner,
        PlayerRef player
        )
    {
        if( !runner.IsServer )
        {
            return;
        }

        if( runner.ActivePlayers.Count() == 1 )
        {
            _gameManager = runner.Spawn( _gameManagerPrefab, Vector3.zero, Quaternion.identity );
        }

        NetworkPlayerController controller = runner.Spawn( _networkPlayerControllerPrefab, Vector3.zero, Quaternion.identity, inputAuthority: player );
        controller.Initialize( (ETeam)_playerCount++ + 1 ); //dumb
    }

    private void ConnexionHandler_OnPlayerLeft(
        NetworkRunner runner,
        PlayerRef player
        )
    {
        Debug.Log( $"Player {player} left the game, unloading scenario" );

        ScenarioManager.Instance.UnloadScenarioAsync().Forget();
    }

    private void ConnexionHandler_OnDisconnectedFromServer(
        NetworkRunner runner,
        NetDisconnectReason disconnect_reason
        )
    {
        Debug.Log( $"Disconnected from server: {disconnect_reason}, unloading scenario" );

        ScenarioManager.Instance.UnloadScenarioAsync().Forget();
    }
}
