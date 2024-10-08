using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static bool HasInstance => _instance != null;

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
