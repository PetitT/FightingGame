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

    public void OnFixedUpdateNetwork(
        int tick
        )
    {
        ReadInputs( tick );
    }

    private void ReadInputs(
        int tick
        )
    {
        Inputs inputs = new Inputs(
            _team,
            (int)_inputs.Character.Position.ReadValue<float>(),
            (int)_inputs.Character.Movement.ReadValue<float>(),
            (int)_inputs.Character.Attack.ReadValue<float>() > 0
            );

        RPC_SendInput( Runner, inputs.ToByte(), tick );
    }

    [Rpc( RpcSources.All, RpcTargets.All )]
    public static void RPC_SendInput(
        NetworkRunner runner,
        byte input,
        int tick,
        RpcInfo info = default
        )
    {
        OnInputReceived.Invoke( new InputReceivedArgs( tick, new Inputs( input ), info.IsInvokeLocal ) );
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
