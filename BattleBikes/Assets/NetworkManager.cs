using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    [SerializeField]
    private Text nickname, status, room, players, joinOrLeave;
    [SerializeField]
    private Button buttonPlay, buttonLeave;
    [SerializeField]
    private InputField playerName;
    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    public GameObject player, sidePanel, custPanel, gamePanel, winPanel, losePanel;
    public Camera main;
    public AudioSource menuMusic, gameMusic;

    public int myNumber;
    string myName;
    bool gameStarted;

    public string[] playerNames;
    public Text[] playerNameTexts;
    public int[] playerScores;
    public int scoreTotal;
    public Text[] playerScoreText;
    public Transform[] scoreOrder;

    void Start()
    {
        instance = this;
        status.text = "Connecting...";
        nickname.text = "";
        buttonPlay.gameObject.SetActive(false);
        buttonLeave.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
        playerName.gameObject.SetActive(false);
        gameMusic.Stop();
        menuMusic.Play();
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public void Play()
    {
        PlayerPrefs.SetString("PlayerName", playerName.text);
        PhotonNetwork.NickName = playerName.text;
        if (playerName.text == "")
        {
            myName = ("Blank");
        }
        else
        {
            myName = PhotonNetwork.NickName;
        }
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating new room...");

        int randRoomNum = Random.Range(0, 10000); //random name for room
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayersPerRoom, CleanupCacheOnLeave = true };
        PhotonNetwork.CreateRoom("BB" + randRoomNum, roomOps); // failed to join a random room, so create a new one
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create a room... trying again...");
        CreateRoom();
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
        Time.timeScale = 1;
        gameStarted = false;
        main.gameObject.SetActive(true);
        gameMusic.Stop();
        menuMusic.Play();
        buttonLeave.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster was called by PUN.");
        status.text = "Connected to Photon.";
        nickname.text = "Type your name and click PLAY!";
        playerName.text = PlayerPrefs.GetString("PlayerName");
        sidePanel.gameObject.SetActive(true);
        custPanel.gameObject.SetActive(true);
        buttonPlay.gameObject.SetActive(true);
        playerName.gameObject.SetActive(true);

        var clones = GameObject.FindGameObjectsWithTag("Track");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers) 
        { 
            buttonPlay.gameObject.SetActive(false);
            playerName.gameObject.SetActive(false);
            nickname.text = "Waiting for more players... " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        }
        else
        {
            photonView.RPC("StartGame", RpcTarget.All);
        }

    }
    [PunRPC] private void StartGame()
    {
        main.gameObject.SetActive(false);
        Debug.Log("Yep, you managed to join a room!");
        joinOrLeave.text = PhotonNetwork.NickName + " has joined the battle.";

        sidePanel.gameObject.SetActive(false);
        custPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);
        buttonLeave.gameObject.SetActive(true);

        menuMusic.Stop();
        gameMusic.Play();

        myNumber = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        for (int i = 0; i < 4; i++)
        {
            playerScores[i] = 30;
        }

        gameStarted = true;

        PhotonNetwork.Instantiate(player.name, SpawnPoints.instance.spawnPoints[myNumber].position, SpawnPoints.instance.spawnPoints[myNumber].rotation);
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        joinOrLeave.text = newPlayer.NickName + " has joined the battle.";
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        joinOrLeave.text = otherPlayer.NickName + " has left the battle.";
        otherPlayer.SetScore(0);
    }

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < 4)
            {
                nickname.text = "Waiting for more players... " + PhotonNetwork.CurrentRoom.PlayerCount + " / 4";
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1 && gameStarted == true)
            {
                Time.timeScale = 0;
                winPanel.gameObject.SetActive(true);
            }

            if (playerScores[myNumber] <= 0 && gameStarted == true)
            {
                Time.timeScale = 0;
                losePanel.gameObject.SetActive(true);
            }

                room.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
            players.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
            photonView.RPC("SyncScoreboard", RpcTarget.All, myNumber, myName);
            ScoreboardUpdate();
        }
        else if (PhotonNetwork.IsConnected)
        {
            room.text = "Not yet in a room...";
            players.text = "Players: 0";
        }
        else
        {
            nickname.text = room.text = players.text = "";
        }


    }
    [PunRPC] void SyncScoreboard(int num, string nam) 
    {
        playerNames[num] = nam;
        playerNameTexts[num].text = nam;
    }

    void ScoreboardUpdate()
    {

        int tempTotal = 0;

        for (int i = 0; i < playerScores.Length; i++)
        {
            tempTotal += playerScores[i];
        }

        if (tempTotal != scoreTotal)
        {
            OrderUpdate();
            scoreTotal = tempTotal;
            for (int i = 0; i < playerScores.Length; i++)
            {
                playerScoreText[i].text = playerScores[i].ToString();
            }
        }
    }

    public void OrderUpdate()
    {
        Transform[] order = scoreOrder;
        int[] scores = playerScores;
        int[] places = { 0, 0, 0, 0};
        for (int i = 0; i < scores.Length; i++)
        {
            for (int j = 0; j < scores.Length; j++)
            {
                if(scores[i] < scores[j])
                {
                    places[i]++;
                }
            }
        }
        for (int i = 0; i < order.Length; i++)
        {
            order[i].SetSiblingIndex(places[i]);
        }
    }
}
