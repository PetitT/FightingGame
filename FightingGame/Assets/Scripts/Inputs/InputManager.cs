using Fusion;
using UnityEngine;

public class InputManager : NetworkBehaviour
{
    [SerializeField] private int InputDelay = 3;
    private InputHandler _inputHandler = null;

    private bool Initialized = false;

    public void Initialize( 
        InputReceiverManager input_receiver_manager,
        GameStateManager game_state_manager
        )
    {
        _inputHandler = new InputHandler( game_state_manager, input_receiver_manager, ScenarioManager.Instance.ActiveScenario.ConnexionHandler.PlayerCount, InputDelay );
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

        Debug.Log( $"Input manager fixed update {tick}" );
        _inputHandler.OnFixedUpdateNetwork( tick );
    }

    private void InputReader_OnInputReceived(
        InputReceivedArgs args
        )
    {
        _inputHandler.ReceiveInput( args );
    }
}
