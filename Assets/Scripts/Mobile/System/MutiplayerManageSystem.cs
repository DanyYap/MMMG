using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;

public class MultiplayerManageSystem : MonoBehaviourPunCallbacks
{
    #region Public Variables
    public static string LastCreatedLobbyNumber;
    private List<string> availableRooms = new List<string>(); // To track available rooms
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        // Connect to Photon
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion

    #region Photon Callbacks
    public override void OnConnected()
    {
        // Connected to internet!
        Debug.Log("CONNECTED to internet");
    }

    public override void OnConnectedToMaster()
    {
        // Connected to server!
        Debug.Log("CONNECTED to the server");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // Disconnected from server!
        Debug.Log("DISCONNECTED from the server");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // Room creation failed
        Debug.Log("Room creation failed: " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    
        // Add the newly created room to availableRooms
        if (!availableRooms.Contains(PhotonNetwork.CurrentRoom.Name))
        {
            availableRooms.Add(PhotonNetwork.CurrentRoom.Name);
            Debug.Log("Added room to available rooms: " + PhotonNetwork.CurrentRoom.Name);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnLeftRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " left the room");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed: " + message);
    }

    //public override void on

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        availableRooms.Clear();
        foreach (var roomInfo in roomList)
        {
            if (roomInfo.IsOpen && roomInfo.IsVisible)
            {
                availableRooms.Add(roomInfo.Name);
            }
        }
        Debug.Log("Updated available rooms: " + string.Join(", ", availableRooms));
    }
    #endregion

    #region Connection Management
    public void OnConnectToPhotonServerButtonClick()
    {
        // Check if already connected before attempting to connect
        if (!PhotonNetwork.IsConnected)
        {
            // Connect to Photon
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnDisconnectButtonClick()
    {
        // Disconnect from Photon
        PhotonNetwork.Disconnect();
    }
    #endregion

    #region Lobby Creation
    public void OnCreateLobbyButtonClick(TMP_Text lobbyText)
    {
        // Check if connected
        if (!PhotonNetwork.IsConnectedAndReady) return;

        // Create a new lobby number
        LastCreatedLobbyNumber = Random.Range(1, 99999).ToString("D5");

        // Set up room options
        var lobbyOptions = new RoomOptions
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 2
        };

        // Create room
        PhotonNetwork.CreateRoom(LastCreatedLobbyNumber, lobbyOptions);
        UpdateLobbyText(lobbyText, LastCreatedLobbyNumber);
    }
    #endregion

    #region Lobby Joining
   public bool OnJoinLobbyButtonClick(string roomNumber)
{
    // Check if room number is valid
    if (string.IsNullOrEmpty(roomNumber))
    {
        Debug.Log("Cannot join room. Room number is empty.");
        return false; // Failed
    }

    // Check if the room exists in the available rooms
    if (availableRooms.Contains(roomNumber))
    {
        Debug.Log("Joined room: " + roomNumber);
        
        // Join room
        PhotonNetwork.JoinRoom(roomNumber);
        return true; 
    }

    Debug.Log("Cannot join room. Room number does not exist in the available rooms.");
    return false; // Failed
}



    public void OnLeaveLobbyButtonClick(TMP_Text lobbyText)
    {
        // Check if in a room
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            UpdateLobbyText(lobbyText, "Lobby");
        }
        else
        {
            Debug.Log("Not in a room to leave.");
        }
    }
    #endregion

    #region Private Methods
    private void UpdateLobbyText(TMP_Text lobbyText, string text)
    {
        // Update lobby text
        if (lobbyText != null)
        {
            lobbyText.text = text;
        }
        else
        {
            Debug.LogError("Lobby text reference is not set.");
        }
    }
    #endregion
}
