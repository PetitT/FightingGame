using UnityEngine;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private GameStartManager _gameStartManager;
    private GameStateManager _gameStateManager = new();
    private InputReceiverManager _inputReceiverManager = new();

    public GameStartManager GameStartManager => _gameStartManager;
    public GameStateManager GameStateManager => _gameStateManager;
    public InputReceiverManager InputReceiverManager => _inputReceiverManager;
}
