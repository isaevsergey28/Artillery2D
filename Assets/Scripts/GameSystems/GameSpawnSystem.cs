using System.Collections.Generic;
using UnityEngine;

public class GameSpawnSystem : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnDots;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _botPrefab;

    private SpawnPoint[] _spawnPoints;
    private Vector3 _spawnPos;
    private List<int> _participantWalkNumbers = new List<int>();

    private int _busySpawns = 0;
    private int _spawnNumber;
   

    private void Awake()
    {
        int num = 0;
        _spawnPoints = new SpawnPoint[_spawnDots.Length];
        foreach (var spawnDot in _spawnDots)
        {
            _spawnPoints[num++] = spawnDot.GetComponent<SpawnPoint>();
        }
        SpawnPlayer();
        SpawnBot();
    }
    private void SpawnPlayer()
    {
        _spawnNumber = Random.Range(0, _spawnPoints.Length);
        FindSpawnPosAndReserveIt();
        GameObject player = Instantiate(_playerPrefab, _spawnPos, Quaternion.identity, null);
        if(player.TryGetComponent<Participant>(out Participant participant))
        {
            int walkNumber = Random.Range(1, _spawnDots.Length + 1);
            participant.SetWalkNumber(walkNumber);
            if(_participantWalkNumbers.Count == _spawnDots.Length)
            {
                _participantWalkNumbers.Clear();
            }
            _participantWalkNumbers.Add(walkNumber);
        }
        _busySpawns = 1;
    }

    private void SpawnBot()
    {
        while (_busySpawns != _spawnPoints.Length)
        {
            _spawnNumber = Random.Range(0, _spawnPoints.Length);
            if (_spawnPoints[_spawnNumber].GetStatus() == false)
            {
                FindSpawnPosAndReserveIt();
                 GameObject bot = Instantiate(_botPrefab, _spawnPos, Quaternion.identity, null);
                if (bot.TryGetComponent<Participant>(out Participant participant))
                {
                    while(true)
                    {
                        int walkNumber = Random.Range(1, _spawnDots.Length + 1);
                        if(_participantWalkNumbers.Contains(walkNumber))
                        {
                            continue;
                        }
                        participant.SetWalkNumber(walkNumber);
                        _participantWalkNumbers.Add(walkNumber);
                        break;
                    }
                }
                _busySpawns++;
            }
        }
    }
    
    private void FindSpawnPosAndReserveIt()
    {
        _spawnPos = _spawnPoints[_spawnNumber].gameObject.transform.position;
        _spawnPoints[_spawnNumber].ReservePoint();
    }
    public int GetPlayersCount()
    {
        return _spawnDots.Length;
    }
}
