using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    private Text _gameOverText;

    private void OnEnable()
    {
        _gameOverText = gameObject.GetComponentInChildren<Text>();
    }
    public void ShowPanel(Participant participant)
    {
        _gameOverText.text += "\n" + participant.gameObject.transform.GetChild(0).GetComponentInChildren<Text>().text;
    }
}
