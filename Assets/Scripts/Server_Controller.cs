using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Server_Controller : MonoBehaviour
{
    public enum MessageType
    {   
        PlayerConnected = 50,
        PlayerDisconnected = 51,
        PlayerLocation = 52,
        WelcomePlayer = 53
    }

    public class Client
    {
        public int connectionId;
        public NetworkConnection Connection;
        public GameObject Object;
        public Vector3 Location;
    }

    public class LocationPacket : MessageBase
    {
        public int Id;
        public Vector3 Location;
    }

    public class PlayerStatusPacket : MessageBase
    {
        public int connectionId;
    }

    public class PlayerWelcomePacket : MessageBase
    {
        public int ConnectionId;
        public LocationPacket[] Players;
    }

    private NetworkManager_Server_Controller _networkManager
    { get { return GetComponent<NetworkManager_Server_Controller>(); } }

    private List<Client> _clients
    { get { return _networkManager.Clients; } }

    void Start()
    {
        _networkManager.StartHost();
    }
}
