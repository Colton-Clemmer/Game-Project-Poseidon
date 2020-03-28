using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server_Controller : MonoBehaviour
{
    public int ReliableChannelId;
    public int UnreliableChannelId;
    public int HostId;

    void Start()
    {
        // NetworkTransport.Init();
        // var config = new ConnectionConfig();
        // ReliableChannelId = config.AddChannel(QosType.Reliable);
        // UnreliableChannelId = config.AddChannel(QosType.UnReliable);
        // var topology = new HostTopology(config, 10);
        // HostId = NetworkTransport.AddHost(topology, 8888);

        // var server = new NetworkServer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
