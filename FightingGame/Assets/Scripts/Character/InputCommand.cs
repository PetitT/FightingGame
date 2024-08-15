public readonly struct InputCommand
{
    public EInputDirection Direction { get; }
    public bool IsAttacking { get; }

    public InputCommand(
        Inputs input
        )
    {
        Direction = input.GetInputDirection();
        IsAttacking = input.IsAttacking;
    }
}
