using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using Mirror;

public class Client_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _remotePlayer;
    [SerializeField]
    private Transform _spawnPosition;
    [SerializeField]
    public vThirdPersonCamera _camera;
    [SerializeField]
    private float _maxSpawnDistance = 5;
    public GameObject Player;
    private NetworkManager_Client_Controller _networkManager
    { get { return GetComponent<NetworkManager_Client_Controller>(); } }

    // Start is called before the first frame update
    void Start()
    {
        _networkManager.StartClient();
    }

    public void SpawnLocalPlayer()
    {
        Player = Instantiate(_player);
        Player.transform.parent = gameObject.transform;
        Player.transform.position = new Vector3(
            _spawnPosition.position.x + (UnityEngine.Random.value * _maxSpawnDistance),
            _spawnPosition.position.y + (UnityEngine.Random.value * _maxSpawnDistance),
            10
        );
        _camera.target = Player.transform;
    }

    public GameObject SpawnRemotePlayer()
    {
        var player = Instantiate(_remotePlayer);
        player.transform.parent = gameObject.transform;
        player.transform.position = new Vector3(
            _spawnPosition.position.x + (UnityEngine.Random.value * _maxSpawnDistance),
            _spawnPosition.position.y + (UnityEngine.Random.value * _maxSpawnDistance),
            10
        );
        return player;
    }

    // Update is called once per frame
    void Update()
    {
        if (_networkManager.Connection != null) _networkManager.SendLocation();
    }
}