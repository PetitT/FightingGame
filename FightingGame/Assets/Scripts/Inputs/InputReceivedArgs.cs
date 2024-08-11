public readonly struct InputReceivedArgs
{
    public int Tick { get; }
    public Inputs Inputs { get; }
    public bool IsLocalInput { get; }

    public InputReceivedArgs(
        int tick,
        Inputs inputs,
        bool is_local_input
        )
    {
        Tick = tick;
        Inputs = inputs;
        IsLocalInput = is_local_input;
    }
}
