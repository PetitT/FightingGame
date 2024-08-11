using System;

[Serializable]
public struct Inputs
{
    // Position and Movement may only be -1, 0, or 1

    public ETeam Team;
    public int Position;
    public int Movement;
    public bool IsAttacking;

    public Inputs(
        ETeam team,
        int position,
        int movement,
        bool is_attacking
        )
    {
        Team = team;
        Position = position;
        Movement = movement;
        IsAttacking = is_attacking;
    }

    public Inputs(
        byte input
        )
    {
        Team = (ETeam)( ( input >> 7 ) & 0b1 );
        Position = ( ( input >> 5 ) & 0b11 ) - 1; // Subtract 1 to get the original value
        Movement = ( ( input >> 3 ) & 0b11 ) - 1;
        IsAttacking = Convert.ToBoolean( ( input >> 2 ) & 0b1 );
    }

    public static Inputs None()
    {
        return new Inputs( ETeam.None, 0, 0, false );
    }

    public byte ToByte()
    {
        byte input = 0;

        input |= (byte)( ( (int)Team & 0b1 ) << 7 );
        input |= (byte)( ( ( Position + 1 ) & 0b11 ) << 5 ); // Add 1 to avoid storing negative values
        input |= (byte)( ( ( Movement + 1 ) & 0b11 ) << 3 );
        input |= (byte)( ( Convert.ToByte( IsAttacking ) & 0b1 ) << 2 );

        return input;
    }

    public override string ToString()
    {
        return $"Team: {Team}, Position: {Position}, Movement: {Movement}, IsAttacking: {IsAttacking}";
    }
}
