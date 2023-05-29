using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ExtendedNetworkManager : NetworkManager
{
    public GameObject NetworkPlayerPrefab;
    public void ReplacePlayer(NetworkConnectionToClient conn)
    {
        GameObject oldPlayer = conn.identity.gameObject;
        NetworkServer.ReplacePlayerForConnection(conn, Instantiate(NetworkPlayerPrefab), true);
        Destroy(oldPlayer, 0.1f);
    }

}
