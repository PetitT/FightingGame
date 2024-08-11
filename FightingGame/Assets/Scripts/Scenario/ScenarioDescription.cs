using UnityEngine;
using Fusion;

[CreateAssetMenu( fileName = "ScenarioDescription", menuName = "Game/ScenarioDescription" )]
public class ScenarioDescription : ScriptableObject
{
    [SerializeField] private AssetReferenceScene _sceneReference = null;
    [SerializeField] private bool _isOnline = false;
    [SerializeField] private string _sessionName = string.Empty;

    public AssetReferenceScene SceneReference => _sceneReference;
    public GameMode GameMode => _isOnline ? GameMode.AutoHostOrClient : GameMode.Single;
    public int PlayerCount => _isOnline ? 2 : 1;
    public string SessionName => _sessionName;
}
