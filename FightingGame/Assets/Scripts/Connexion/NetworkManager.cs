using Cysharp.Threading.Tasks;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    private const int PLAYER_COUNT = 2;

    [SerializeField] private ConnexionHandler _connexionHandlerPrefab = null;

    public async UniTask<(bool, ConnexionHandler)> CreateConnexion(
        ConnectionParameters connection_parameters
        )
    {
        ConnexionHandler new_connexion_handler = Instantiate( _connexionHandlerPrefab );

        bool has_connected = await new_connexion_handler.Connect( connection_parameters );

        if( !has_connected )
        {
            Destroy( new_connexion_handler.gameObject );

            return (false, null);
        }

        return (true, new_connexion_handler);
    }
}