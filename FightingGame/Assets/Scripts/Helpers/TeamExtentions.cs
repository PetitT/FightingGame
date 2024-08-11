public static class TeamExtentions
{
    public static ETeam GetOppositeTeam( this ETeam team )
    {
        return team == ETeam.TeamOne ? ETeam.TeamTwo : ETeam.TeamOne;
    }

    public static bool IsSameTeam( this ETeam team, ETeam otherTeam )
    {
        return team == otherTeam;
    }
}
