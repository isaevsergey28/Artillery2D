using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform[] _spawnDots;
    private SpawnPoint[] _spawnPoints;
    private Vector3 _spawnPos;
    
    private int _pointNumber;

    private void Awake()
    {
        int num = 0;
        _spawnPoints = new SpawnPoint[_spawnDots.Length];
        foreach (var spawnDot in _spawnDots)
        {
            _spawnPoints[num++] = spawnDot.GetComponent<SpawnPoint>();
        }
    }

    private void Start()
    {
        int num;
        while (true)
        {
            num = Random.Range(0, _spawnPoints.Length);
            if (_spawnPoints[num].GetStatus() == false)
            {
                _spawnPos = _spawnPoints[num].gameObject.transform.position;
                break;
            }
        }

        PhotonNetwork.Instantiate(_player.name, _spawnPos, Quaternion.identity);
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }
}
