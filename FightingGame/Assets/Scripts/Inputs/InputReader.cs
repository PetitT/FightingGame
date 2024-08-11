using Fusion;
using UnityEngine.Events;

public class InputReader : NetworkBehaviour
{
    private GameInputs _inputs = null;
    private ETeam _team = ETeam.None;

    public static UnityEvent<InputReceivedArgs> OnInputReceived = new();

    public void Initialize(
        ETeam team
        )
    {
        _team = team;
    }

    public override void FixedUpdateNetwork()
    {
        ReadInputs();
    }

    private void ReadInputs()
    {
        Inputs inputs = new Inputs(
            _team,
            (int)_inputs.Character.Position.ReadValue<float>(),
            (int)_inputs.Character.Movement.ReadValue<float>(),
            (int)_inputs.Character.Attack.ReadValue<float>() > 0
            );

        RPC_SendInput( Runner, inputs.ToByte() );
    }

    [Rpc( RpcSources.All, RpcTargets.All )]
    public static void RPC_SendInput(
        NetworkRunner runner,
        byte input,
        RpcInfo info = default
        )
    {
        OnInputReceived.Invoke( new InputReceivedArgs( info.Tick.Raw, new Inputs( input ), info.IsInvokeLocal ) );
    }

    private void Awake()
    {
        _inputs = new GameInputs();
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
}
