using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using MessageType = Server_Controller.MessageType;
using LocationPacket = Server_Controller.LocationPacket;
using PlayerStatusPacket = Server_Controller.PlayerStatusPacket;
using PlayerWelcomePacket = Server_Controller.PlayerWelcomePacket;
using Client = Server_Controller.Client;

public class NetworkManager_Client_Controller : NetworkManager
{
    private Client_Controller _client
    { get { return GetComponent<Client_Controller>(); } }

    public NetworkConnection Connection;
    public int ConnectionId;
    public List<Client> Clients = new List<Client>();
    
    public override void OnClientConnect(NetworkConnection conn)
    {
        _client.SpawnLocalPlayer();
        Connection = conn;
        _registerHandlers();
        SendLocation();
    }

    public void SendLocation()
    {
        NetworkClient.Send((int) MessageType.PlayerLocation, new LocationPacket { Location = _client.Player.transform.position });
    }

    private void _registerHandlers()
    {
        NetworkClient.RegisterHandler((int) MessageType.PlayerConnected, _addPlayerFromNetwork);
        NetworkClient.RegisterHandler((int) MessageType.PlayerDisconnected, _removePlayer);
        NetworkClient.RegisterHandler((int) MessageType.PlayerLocation, _updatePlayerLocation);
        NetworkClient.RegisterHandler((int) MessageType.WelcomePlayer, _initilizeScene);
    }

    private static void _updatePlayerLocation(NetworkMessage msg)
    {
        var manager = GameObject.FindGameObjectWithTag("Client");
        var nm = manager.GetComponent<NetworkManager_Client_Controller>();
        var message = msg.ReadMessage<LocationPacket>();
        if (message.Id == nm.Connection.connectionId) return;
        var client = nm.Clients.FirstOrDefault(c => c.connectionId == message.Id);
        if (client == null) return;
        client.Object.transform.position = message.Location;
    }

    private static void _addPlayerFromNetwork(NetworkMessage msg)
    {
        var message = msg.ReadMessage<PlayerStatusPacket>();
        var nm = GameObject.FindGameObjectWithTag("Client").GetComponent<NetworkManager_Client_Controller>();
        if (nm.ConnectionId == message.connectionId) return;
        _addPlayer(message.connectionId);
    }

    private static void _addPlayer(int connectionId, Vector3? location = null)
    {
        var nm = GameObject.FindGameObjectWithTag("Client").GetComponent<NetworkManager_Client_Controller>();
        if (nm.ConnectionId == connectionId) return;
        nm.Clients.Add(new Client{
            connectionId = connectionId,
            Object = nm.GetComponent<Client_Controller>().SpawnRemotePlayer(),
            Location = location ?? Vector3.zero
        });
    }

    private static void _removePlayer(NetworkMessage msg)
    {
        var message = msg.ReadMessage<PlayerStatusPacket>();
        var nm = GameObject.FindGameObjectWithTag("Client").GetComponent<NetworkManager_Client_Controller>();
        var client = nm.Clients.FirstOrDefault(c => c.Connection.connectionId == message.connectionId);
        if (client == null) return;
        Destroy(client.Object);
        nm.Clients.Remove(client);
    }

    private static void _initilizeScene(NetworkMessage msg)
    {
        var message = msg.ReadMessage<PlayerWelcomePacket>();
        var nm = GameObject.FindGameObjectWithTag("Client").GetComponent<NetworkManager_Client_Controller>();
        nm.ConnectionId = message.ConnectionId;
        foreach(var player in message.Players)
        {
            _addPlayer(player.Id, player.Location);
        }
    }
}
