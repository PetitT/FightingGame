using Fusion;
using UnityEngine;

public class InputManager : NetworkBehaviour
{
    [SerializeField] private GameDatas _gameDatas = null;
    private InputHandler<GameStateMatch> _inputHandler = null;

    private bool Initialized = false;

    public void Initialize( 
        InputReceiverManager input_receiver_manager,
        GameStateManager<GameStateMatch> game_state_manager,
        ETeam local_team
        )
    {
        _inputHandler = new InputHandler<GameStateMatch>(
            game_state_manager, 
            input_receiver_manager,
            ScenarioManager.Instance.ActiveScenario.ConnexionHandler.PlayerCount,
            _gameDatas.InputTickDelay,
            local_team 
            );

        InputReader.OnInputReceived.AddListener( InputReader_OnInputReceived );
        Initialized = true;
    }

    public override void Despawned(
        NetworkRunner runner,
        bool hasState
        )
    {
        Initialized = false;
        InputReader.OnInputReceived.RemoveListener( InputReader_OnInputReceived );
    }

    public void OnFixedUpdateNetwork(
        int tick
        )
    {
        if( !Initialized )
        {
            return;
        }

        _inputHandler.OnFixedUpdateNetwork( tick );
    }

    private void InputReader_OnInputReceived(
        InputReceivedArgs args
        )
    {
        _inputHandler.ReceiveInput( args );
    }
}
