using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;


public class Client_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Transform _spawnPosition;
    [SerializeField]
    public vThirdPersonCamera _camera;
    [SerializeField]
    private float _maxSpawnDistance = 5;
    public GameObject CurrentPlayer;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Spawn()
    {
        CurrentPlayer = Instantiate(_player);
        CurrentPlayer.transform.parent = gameObject.transform;
        CurrentPlayer.transform.position = new Vector3(
            _spawnPosition.position.x + (UnityEngine.Random.value * _maxSpawnDistance),
            _spawnPosition.position.y + (UnityEngine.Random.value * _maxSpawnDistance),
            10
        );
        _camera.target = CurrentPlayer.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}