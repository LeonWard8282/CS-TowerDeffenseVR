using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private string mapType;

    public TextMeshProUGUI occupancyColony;
    public TextMeshProUGUI occupancyScavenger;
    public TextMeshProUGUI occupancyCorporation;
    public TextMeshProUGUI occupancyExplorer;
    public TextMeshProUGUI occupancyBlackMarket;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if(!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UI Callback Methods
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnEnterButtonClicked_Colony()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_COLONY;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY,mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);

    }

    public void OnEnterButtonClicked_Scavenger()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_SCAVENGER;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterButtonClicked_Explorer()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_EXPLORER;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterButtonClicked_Corporation()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_CORPORATION;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterButtonClicked_BlackMarket()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_BLACKMARKET;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    #endregion

    #region Photon Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateAndJoinRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("A room is created with name: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("The local player: " + PhotonNetwork.NickName + "joined to " + PhotonNetwork.CurrentRoom.Name + "Player Count " + PhotonNetwork.CurrentRoom.PlayerCount);

        if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
        {
            object mapType;
            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out mapType))
            {
                Debug.Log("Joined room with map: " + (string)mapType);
                if((string) mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_COLONY)
                {
                    PhotonNetwork.LoadLevel("Colony");
                }
                if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_EXPLORER)
                {
                    PhotonNetwork.LoadLevel("Explorer");
                }
                if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_CORPORATION)
                {
                    PhotonNetwork.LoadLevel("Corporation");
                }
                if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_BLACKMARKET)
                {
                    PhotonNetwork.LoadLevel("BlackMarket");
                }
                else
                {
                    PhotonNetwork.LoadLevel("Scavenger");
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "has joined. " + "Player Count" + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(roomList.Count == 0)
        {
            occupancyColony.text = 0 + "/" + 20;
            occupancyScavenger.text = 0 + "/" + 20;
            occupancyExplorer.text = 0 + "/" + 20;
            occupancyCorporation.text = 0 + "/" + 20;
            occupancyBlackMarket.text = 0 + "/" + 20;
        }

        foreach(RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if(room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_COLONY))
            {
                occupancyColony.text = room.PlayerCount + "/" + 20;
            }
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_SCAVENGER))
            {
                occupancyScavenger.text = room.PlayerCount + "/" + 20;
            }
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_EXPLORER))
            {
                occupancyExplorer.text = room.PlayerCount + "/" + 20;
            }
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_CORPORATION))
            {
                occupancyCorporation.text = room.PlayerCount + "/" + 20;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_BLACKMARKET))
            {
                occupancyBlackMarket.text = room.PlayerCount + "/" + 20;
            }
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");
    }

    #endregion

    #region Private Methods
    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room" + mapType +  Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;

        string[] roomPropsInLobby = {MultiplayerVRConstants.MAP_TYPE_KEY};

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY,mapType} };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }


    #endregion
}