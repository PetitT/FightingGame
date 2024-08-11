using UnityEngine;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private GameStartManager _gameStartManager;
    [SerializeField] private GameStateManager _gameStateManager;

    public GameStartManager GameStartManager => _gameStartManager;
    public GameStateManager GameStateManager => _gameStateManager;
}
