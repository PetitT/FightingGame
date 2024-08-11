using Fusion;

public class NetworkSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if( _instance == null )
            {
                _instance = FindFirstObjectByType<T>();
            }

            return _instance;
        }
    }
}
