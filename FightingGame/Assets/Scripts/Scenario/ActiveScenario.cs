using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;

public class ActiveScenario
{
    private SceneInstance _sceneInstance = default;
    private ConnexionHandler _currentConnexion = null;
    private NetworkEventsListener _networkEventsListener = new();

    public ConnexionHandler ConnexionHandler => _currentConnexion;
    public NetworkEventsListener NetworkEventsListener => _networkEventsListener;

    public async UniTask<bool> LoadScenarioAsync(
        ScenarioDescription scenario_description
        )
    {
        _sceneInstance = await SceneManager.Instance.LoadSceneAsync( scenario_description.SceneReference, LoadSceneMode.Additive );

        (bool result, ConnexionHandler connexion_handler) = await NetworkManager.Instance.CreateConnexion( 
                                                                        new ConnectionParameters(
                                                                            scenario_description.GameMode, 
                                                                            scenario_description.PlayerCount, 
                                                                            _networkEventsListener,
                                                                            scenario_description.SessionName 
                                                                            ) 
                                                                        );

        if( !result )
        {
            return false;
        }

        _currentConnexion = connexion_handler;

        return true;
    }

    public async UniTask UnloadScenarioAsync()
    {
        _currentConnexion?.Disconnect();
        await SceneManager.Instance.UnloadSceneAsync( _sceneInstance );
    }
}
