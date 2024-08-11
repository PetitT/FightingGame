using Fusion;
using UnityEngine;

public class InputManager : NetworkBehaviour
{
    private InputHandler _inputHandler = new InputHandler();

    private bool Initialized = false;

    public void Initialize()
    {
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

    public override void FixedUpdateNetwork()
    {
        if( !Initialized )
        {
            return;
        }

        Debug.Log( $"Calling InputHandler {Runner.Tick.Raw}" );
        _inputHandler.OnFixedUpdateNetwork( Runner.Tick.Raw );
    }

    private void InputReader_OnInputReceived(
        InputReceivedArgs args
        )
    {
        _inputHandler.ReceiveInput( args );
    }
}
