using UnityEngine;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private GameStartManager _gameStartManager = null;
    [SerializeField] private CharacterManager _characterManager = null;
    private GameStateManager<GameStateMatch> _gameStateManager = new();
    private InputReceiverManager _inputReceiverManager = new();

    public GameStartManager GameStartManager => _gameStartManager;
    public CharacterManager CharacterManager => _characterManager;
    public GameStateManager<GameStateMatch> GameStateManager => _gameStateManager;
    public InputReceiverManager InputReceiverManager => _inputReceiverManager;
}
