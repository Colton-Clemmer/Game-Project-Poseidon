using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using MessageType = Server_Controller.MessageType;
using LocationPacket = Server_Controller.LocationPacket;
using PlayerStatusPacket = Server_Controller.PlayerStatusPacket;
using PlayerWelcomePacket = Server_Controller.PlayerWelcomePacket;
using Client = Server_Controller.Client;

public class NetworkManager_Server_Controller : NetworkManager
{
    public List<Client> Clients = new List<Client>();

    public override void OnServerConnect(NetworkConnection conn)
    {   
        _registerHandlers();
        if (conn.connectionId == 0) return;
        Clients.Add(new Client { connectionId = conn.connectionId, Connection = conn });

        NetworkServer.SendToClient(conn.connectionId, (int) MessageType.WelcomePlayer, new PlayerWelcomePacket{
            ConnectionId = conn.connectionId,
            Players = Clients.Where(c => c.connectionId != conn.connectionId).Select(c => new LocationPacket { Id = c.connectionId, Location = c.Location }).ToArray()
        });
        NetworkServer.SendToAll((int) MessageType.PlayerConnected, new PlayerStatusPacket{ connectionId = conn.connectionId });
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        var nm = GameObject.FindGameObjectWithTag("Server").GetComponent<NetworkManager_Server_Controller>();
        var client = nm.Clients.FirstOrDefault(c => c.Connection.connectionId == conn.connectionId);
        if (client == null) return;
        nm.Clients.Remove(client);
    }

    private void _registerHandlers()
    {
        NetworkServer.RegisterHandler((int) MessageType.PlayerLocation, _serverUpdateLocation);
    }

    private static void _serverUpdateLocation(NetworkMessage msg)
    {
        var Server = GameObject.FindGameObjectWithTag("Server");
        var nm = Server.GetComponent<NetworkManager_Server_Controller>();
        var client = nm.Clients.FirstOrDefault(c => c.Connection.connectionId == msg.conn.connectionId);
        if (client == null) return;
        client.Location = msg.ReadMessage<LocationPacket>().Location;
        NetworkServer.SendToAll((int) MessageType.PlayerLocation, new LocationPacket { Id = msg.conn.connectionId, Location = client.Location });
    }
}
