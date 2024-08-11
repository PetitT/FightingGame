using System.Collections.Generic;

public interface IInputReceiver
{
    public void ReceiveInputs( IReadOnlyList<Inputs> inputs );
}
