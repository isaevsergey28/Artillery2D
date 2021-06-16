using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerInfoIndicator : InfoIndicator
{
    private void Start()
    {
        _nickname.text = PlayerProfile.Nickname + " " + _participant.GetHealth() + "HP";
        _nickname.color = PlayerProfile.NicknameColor;
        _participant.onHurt += UpdateIndicator;
    }
    private void OnDisable()
    {
        _participant.onHurt -= UpdateIndicator;
    }
    private void UpdateIndicator(int health)
    {
        _nickname.text = PlayerProfile.Nickname + " " + health + "HP";
    }
}