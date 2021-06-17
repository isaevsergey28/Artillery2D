using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BotInfoIndicator : InfoIndicator
{
    public static int staticBotNumber = 1;

    private int _botNumber;

    private void Start()
    {
        _botNumber = staticBotNumber++;
        _nickname.text = "Bot" + _botNumber + " " + _participant.GetHealth() + "HP";
        _nickname.color = new Color(Random.value, Random.value, Random.value);
        _participant.onHurt += UpdateIndicator;
    }
    private void UpdateIndicator(int health)
    {
        _nickname.text = "Bot" + _botNumber + " " + health + "HP";
    }
}
