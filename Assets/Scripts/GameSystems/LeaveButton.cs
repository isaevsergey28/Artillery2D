using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveButton : MonoBehaviour
{
    public void LoadMenu()
    {
        BotInfoIndicator.staticBotNumber = 1;
        SceneManager.LoadScene("Menu");
    }
}
