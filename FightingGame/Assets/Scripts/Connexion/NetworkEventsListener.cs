using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;

public class NetworkEventsListener : INetworkRunnerCallbacks
{
    public UnityEvent<NetworkRunner> OnConnectedToServerEvent = new UnityEvent<NetworkRunner>();
    public UnityEvent<NetworkRunner, NetAddress, NetConnectFailedReason> OnConnectFailedEvent = new UnityEvent<NetworkRunner, NetAddress, NetConnectFailedReason>();
    public UnityEvent<NetworkRunner, NetworkRunnerCallbackArgs.ConnectRequest, byte[]> OnConnectRequestEvent = new UnityEvent<NetworkRunner, NetworkRunnerCallbackArgs.ConnectRequest, byte[]>();
    public UnityEvent<NetworkRunner, Dictionary<string, object>> OnCustomAuthenticationResponseEvent = new UnityEvent<NetworkRunner, Dictionary<string, object>>();
    public UnityEvent<NetworkRunner, NetDisconnectReason> OnDisconnectedFromServerEvent = new UnityEvent<NetworkRunner, NetDisconnectReason>();
    public UnityEvent<NetworkRunner, HostMigrationToken> OnHostMigrationEvent = new UnityEvent<NetworkRunner, HostMigrationToken>();
    public UnityEvent<NetworkRunner, NetworkInput> OnInputEvent = new UnityEvent<NetworkRunner, NetworkInput>();
    public UnityEvent<NetworkRunner, PlayerRef, NetworkInput> OnInputMissingEvent = new UnityEvent<NetworkRunner, PlayerRef, NetworkInput>();
    public UnityEvent<NetworkRunner, NetworkObject, PlayerRef> OnObjectEnterAOIEvent = new UnityEvent<NetworkRunner, NetworkObject, PlayerRef>();
    public UnityEvent<NetworkRunner, NetworkObject, PlayerRef> OnObjectExitAOIEvent = new UnityEvent<NetworkRunner, NetworkObject, PlayerRef>();
    public UnityEvent<NetworkRunner, PlayerRef> OnPlayerJoinedEvent = new UnityEvent<NetworkRunner, PlayerRef>();
    public UnityEvent<NetworkRunner, PlayerRef> OnPlayerLeftEvent = new UnityEvent<NetworkRunner, PlayerRef>();
    public UnityEvent<NetworkRunner, PlayerRef, ReliableKey, float> OnReliableDataProgressEvent = new UnityEvent<NetworkRunner, PlayerRef, ReliableKey, float>();
    public UnityEvent<NetworkRunner, PlayerRef, ReliableKey, ArraySegment<byte>> OnReliableDataReceivedEvent = new UnityEvent<NetworkRunner, PlayerRef, ReliableKey, ArraySegment<byte>>();
    public UnityEvent<NetworkRunner> OnSceneLoadDoneEvent = new UnityEvent<NetworkRunner>();
    public UnityEvent<NetworkRunner> OnSceneLoadStartEvent = new UnityEvent<NetworkRunner>();
    public UnityEvent<NetworkRunner, List<SessionInfo>> OnSessionListUpdatedEvent = new UnityEvent<NetworkRunner, List<SessionInfo>>();
    public UnityEvent<NetworkRunner, ShutdownReason> OnShutdownEvent = new UnityEvent<NetworkRunner, ShutdownReason>();
    public UnityEvent<NetworkRunner, SimulationMessagePtr> OnUserSimulationMessageEvent = new UnityEvent<NetworkRunner, SimulationMessagePtr>();

    public void OnConnectedToServer( NetworkRunner runner )
    {
        Debug.Log( $"Connected to server: session name = {runner.SessionInfo.Name}" );
        OnConnectedToServerEvent.Invoke( runner );
    }

    public void OnConnectFailed( NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason )
    {
        Debug.Log( $"Failed to connect to server: {reason}" );
        OnConnectFailedEvent.Invoke( runner, remoteAddress, reason );
    }

    public void OnConnectRequest( NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token )
    {
        OnConnectRequestEvent.Invoke( runner, request, token );
    }

    public void OnCustomAuthenticationResponse( NetworkRunner runner, Dictionary<string, object> data )
    {
        OnCustomAuthenticationResponseEvent.Invoke( runner, data );
    }

    public void OnDisconnectedFromServer( NetworkRunner runner, NetDisconnectReason reason )
    {
        Debug.Log( $"Disconnected from server: {reason}" );
        OnDisconnectedFromServerEvent.Invoke( runner, reason );
    }

    public void OnHostMigration( NetworkRunner runner, HostMigrationToken hostMigrationToken )
    {
        OnHostMigrationEvent.Invoke( runner, hostMigrationToken );
    }

    public void OnInput( NetworkRunner runner, NetworkInput input )
    {
        OnInputEvent.Invoke( runner, input );
    }

    public void OnInputMissing( NetworkRunner runner, PlayerRef player, NetworkInput input )
    {
        OnInputMissingEvent.Invoke( runner, player, input );
    }

    public void OnObjectEnterAOI( NetworkRunner runner, NetworkObject obj, PlayerRef player )
    {
        OnObjectEnterAOIEvent.Invoke( runner, obj, player );
    }

    public void OnObjectExitAOI( NetworkRunner runner, NetworkObject obj, PlayerRef player )
    {
        OnObjectExitAOIEvent.Invoke( runner, obj, player );
    }

    public void OnPlayerJoined( NetworkRunner runner, PlayerRef player )
    {
        Debug.Log( $"Player joined: {player} - Player count is {runner.ActivePlayers.Count()}" );
        OnPlayerJoinedEvent.Invoke( runner, player );
    }

    public void OnPlayerLeft( NetworkRunner runner, PlayerRef player )
    {
        Debug.Log( $"Player left: {player} - Player count is {runner.ActivePlayers.Count()}" );
        OnPlayerLeftEvent.Invoke( runner, player );
    }

    public void OnReliableDataProgress( NetworkRunner runner, PlayerRef player, ReliableKey key, float progress )
    {
        OnReliableDataProgressEvent.Invoke( runner, player, key, progress );
    }

    public void OnReliableDataReceived( NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data )
    {
        OnReliableDataReceivedEvent.Invoke( runner, player, key, data );
    }

    public void OnSceneLoadDone( NetworkRunner runner )
    {
        OnSceneLoadDoneEvent.Invoke( runner );
    }

    public void OnSceneLoadStart( NetworkRunner runner )
    {
        OnSceneLoadStartEvent.Invoke( runner );
    }

    public void OnSessionListUpdated( NetworkRunner runner, List<SessionInfo> sessionList )
    {
        OnSessionListUpdatedEvent.Invoke( runner, sessionList );
    }

    public void OnShutdown( NetworkRunner runner, ShutdownReason shutdownReason )
    {
        Debug.Log( $"Shutdown: {shutdownReason}" );
        OnShutdownEvent.Invoke( runner, shutdownReason );
    }

    public void OnUserSimulationMessage( NetworkRunner runner, SimulationMessagePtr message )
    {
        OnUserSimulationMessageEvent.Invoke( runner, message );
    }
}
