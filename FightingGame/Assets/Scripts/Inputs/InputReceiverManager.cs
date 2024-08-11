using System.Collections.Generic;

public class InputReceiverManager
{
    private readonly List<IInputReceiver> _inputReceivers = new();

    public void RegisterInputReceiver(
        IInputReceiver receiver
        )
    {
        _inputReceivers.Add( receiver );
    }

    public void UnregisterInputReceiver(
        IInputReceiver receiver
        )
    {
        _inputReceivers.Remove( receiver );
    }

    public void ApplyInputs(
        IReadOnlyList<Inputs> inputs
        )
    {
        _inputReceivers.ForEach( element => element.ReceiveInputs( inputs ) );
    }
}
