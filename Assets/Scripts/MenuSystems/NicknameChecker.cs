using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameChecker : MonoBehaviour
{
    [SerializeField] private GameObject _startGameButton;

    private GameStarter _gameStarter;
    private Text _nickname;
    
    private void Start()
    {
        _nickname = GetComponentInChildren<Text>();
        _gameStarter = _startGameButton.GetComponent<GameStarter>();
        _gameStarter.onStartGame += CheckNickname;
    }
    private void OnDisable()
    {
        _gameStarter.onStartGame -= CheckNickname;
    }
    private void CheckNickname()
    {
        if (_nickname.text.Length > 3 && _nickname.text.Length < 10)
        {
            _gameStarter.IsNameCorrect = true;
            PlayerProfile.Nickname = _nickname.text;
        }
        else
        {
            _gameStarter.IsNameCorrect = false;
        }
    }
}
