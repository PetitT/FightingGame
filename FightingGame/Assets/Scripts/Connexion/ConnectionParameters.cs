using Fusion;

public readonly struct ConnectionParameters
{
    public GameMode GameMode { get; }
    public int PlayerCount { get; }
    public NetworkEventsListener NetworkEventsListener { get; }
    public string SessionName { get; }

    public ConnectionParameters( 
        GameMode gameMode, 
        int playerCount,
        NetworkEventsListener networkEventsListener,
        string sessionName 
        )
    {
        GameMode = gameMode;
        PlayerCount = playerCount;
        NetworkEventsListener = networkEventsListener;
        SessionName = sessionName;
    }
}
