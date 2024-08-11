using Fusion;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ConnexionHandler : MonoBehaviour
{
    [SerializeField] private NetworkRunner _runner = null;
    public bool IsAuthority => _runner.IsServer;
    public int PlayerCount => _runner.SessionInfo.PlayerCount;
    public bool IsOnline => _runner.GameMode != GameMode.Single;

    public async UniTask<bool> Connect( ConnectionParameters parameters )
    {
        if( parameters.NetworkEventsListener != null )
        {
            _runner.AddCallbacks( parameters.NetworkEventsListener );
        }

        StartGameResult connectionResult = await _runner.StartGame(
            new StartGameArgs
            {
                GameMode = parameters.GameMode,
                PlayerCount = parameters.PlayerCount,
                SessionName = parameters.SessionName
            }
        );

        Debug.Log( connectionResult.Ok ? $"Connected as {_runner.GameMode} - Max player = {_runner.SessionInfo.MaxPlayers}" : $"Failed to connect - {connectionResult.ShutdownReason}" );

        return connectionResult.Ok;
    }

    public void Disconnect()
    {
        Destroy( gameObject );
    }
}
