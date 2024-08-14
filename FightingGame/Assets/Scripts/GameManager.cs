using UnityEngine;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private GameStartManager _gameStartManager;
    private GameStateManager<GameStateMatch> _gameStateManager = new();
    private InputReceiverManager _inputReceiverManager = new();

    public GameStartManager GameStartManager => _gameStartManager;
    public GameStateManager<GameStateMatch> GameStateManager => _gameStateManager;
    public InputReceiverManager InputReceiverManager => _inputReceiverManager;
}
