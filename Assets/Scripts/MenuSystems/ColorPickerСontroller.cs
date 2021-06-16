using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerСontroller : MonoBehaviour
{
    [SerializeField] private GameObject _startGameButton;

    private GameStarter _gameStarter;
    private Toggle[] _allToggles;

   private void Start()
   {
        _allToggles = GetComponentsInChildren<Toggle>();
        _gameStarter = _startGameButton.GetComponent<GameStarter>();
        _gameStarter.onStartGame += CheckForToggle;
    }
    private void OnDisable()
    {
        _gameStarter.onStartGame -= CheckForToggle;
    }
    public void CheckForToggle()
    {
        int includedCount = 0;
        int toggleNumber = 0;

        for(int i = 0; i < _allToggles.Length; i++)
        {
            if(_allToggles[i].isOn == true)
            {
                includedCount++;
                toggleNumber = i;
            }
        }

        if(includedCount == 1)
        {
            _gameStarter.IsColorPicked = true;
            PlayerProfile.NicknameColor = _allToggles[toggleNumber].transform.parent.GetChild(0).GetComponent<Text>().color;
        }
        else
        {
            _gameStarter.IsColorPicked = false;
        }
    }
}
