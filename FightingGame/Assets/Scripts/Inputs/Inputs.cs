using System;
using UnityEngine;

[Serializable]
public struct Inputs
{
    // Vertical and Horizontal may only be -1, 0, or 1

    private int Vertical;
    private int Horizontal;

    public ETeam Team;
    public bool IsAttacking;
    public bool IsMoving => GetMoveDirection() != EMoveDirection.None;

    public Inputs(
        ETeam team,
        int position,
        int movement,
        bool is_attacking
        )
    {
        Team = team;
        Vertical = position;
        Horizontal = movement;
        IsAttacking = is_attacking;
    }

    public Inputs(
        ETeam team
        )
    {
        Team = team;
        Vertical = 0;
        Horizontal = 0;
        IsAttacking = false;
    }

    public Inputs(
        byte input
        )
    {
        Team = (ETeam)( ( input >> 7 ) & 0b1 );
        Vertical = ( ( input >> 5 ) & 0b11 ) - 1; // Subtract 1 to get the original value
        Horizontal = ( ( input >> 3 ) & 0b11 ) - 1;
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
        input |= (byte)( ( ( Vertical + 1 ) & 0b11 ) << 5 ); // Add 1 to avoid storing negative values
        input |= (byte)( ( ( Horizontal + 1 ) & 0b11 ) << 3 );
        input |= (byte)( ( Convert.ToByte( IsAttacking ) & 0b1 ) << 2 );

        return input;
    }

    public EInputDirection GetInputDirection()
    {
        if( Horizontal == 0 && Vertical == 0 ) { return EInputDirection.Neutral; }
        if( Horizontal == 1 && Vertical == 0 ) { return EInputDirection.Right; }
        if( Horizontal == -1 && Vertical == 0 ) { return EInputDirection.Left; }
        if( Vertical == 1 && Horizontal == 0 ) { return EInputDirection.Up; }
        if( Vertical == -1 && Horizontal == 0 ) { return EInputDirection.Down; }
        if( Vertical == 1 && Horizontal == 1 ) { return EInputDirection.UpRight; }
        if( Vertical == 1 && Horizontal == -1 ) { return EInputDirection.UpLeft; }
        if( Vertical == -1 && Horizontal == 1 ) { return EInputDirection.DownRight; }
        if( Vertical == -1 && Horizontal == -1 ) { return EInputDirection.DownLeft; }

        Debug.LogError( $"{this} has an invalid direction" );

        return EInputDirection.Neutral;
    }

    public EMoveDirection GetMoveDirection()
    {
        switch( GetInputDirection() )
        {
            case EInputDirection.Neutral:
            case EInputDirection.Up:
            case EInputDirection.Down:
            default:
                {
                    return EMoveDirection.None;
                }

            case EInputDirection.DownLeft:
            case EInputDirection.Left:
            case EInputDirection.UpLeft:
                {
                    return EMoveDirection.Left;
                }

            case EInputDirection.Right:
            case EInputDirection.UpRight:
            case EInputDirection.DownRight:
                {
                    return EMoveDirection.Right;
                }
        }
    }

    public override string ToString()
    {
        return $"Team: {Team}, Vertical: {Vertical}, Horizontal: {Horizontal}, IsAttacking: {IsAttacking}, Input direction : {GetInputDirection()}, Move direction : {GetMoveDirection()}";
    }
}
