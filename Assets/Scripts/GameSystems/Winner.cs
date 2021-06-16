using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    private GameOverPanel _gameOver;
    private void Start()
    {
        _gameOver = _gameOverPanel.GetComponent<GameOverPanel>();
        AllParticipants.onLastParticipant += StopGame;
    }
    private void OnDisable()
    {
        AllParticipants.onLastParticipant -= StopGame;
    }
    private void StopGame(Participant participant)
    {
        _gameOverPanel.SetActive(true);
        _gameOver.ShowPanel(participant);
    }
}
