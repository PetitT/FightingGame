using Cysharp.Threading.Tasks;

public class ScenarioManager : Singleton<ScenarioManager>
{
    private ActiveScenario _currentScenario = null;

    public ActiveScenario ActiveScenario => _currentScenario;

    public async UniTask<bool> LoadScenarioAsync(
        ScenarioDescription scenario_description
        )
    {
        await UnloadScenarioAsync();

        _currentScenario = new ActiveScenario();
        return await _currentScenario.LoadScenarioAsync( scenario_description );
    }

    public async UniTask UnloadScenarioAsync()
    {
        if( _currentScenario == null )
        {
            return;
        }

        await _currentScenario.UnloadScenarioAsync();
    }
}
