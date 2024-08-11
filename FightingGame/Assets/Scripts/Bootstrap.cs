using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ScenarioDescription _defaultScenario = null;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        LoadPreselectedScenario().Forget();
    }

    private async UniTask LoadPreselectedScenario()
    {
        if( _defaultScenario != null )
        {
            await ScenarioManager.Instance.LoadScenarioAsync( _defaultScenario );
        }
    }
}
