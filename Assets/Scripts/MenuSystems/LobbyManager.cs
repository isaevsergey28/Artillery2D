using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text _logText;
    private void Start()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(1000, 10000);
        Log("Player : " + PhotonNetwork.NickName + " connected.");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Log("Connected to master");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 2});
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Log("Joined room.");
        PhotonNetwork.LoadLevel("SampleScene");
    }

    private void Log(string message)
    {
        Debug.Log(message);
        _logText.text += "\n";
        _logText.text += message;
    }
}
