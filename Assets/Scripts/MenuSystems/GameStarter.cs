using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public bool IsNameCorrect { get; set; }
    public bool IsColorPicked { get; set; }

    public delegate void OnStartGame();
    public event OnStartGame onStartGame;

    public void StartGame()
    {
        onStartGame?.Invoke();

        if(IsColorPicked && IsNameCorrect)
        {
            SceneManager.LoadScene(1);
            ///////////sdfafdsgfdg
        }
    }
}

