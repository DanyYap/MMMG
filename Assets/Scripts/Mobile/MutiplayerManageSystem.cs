using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MultiplayerManageSystem : MonoBehaviourPunCallbacks
{
    public static string LastCreatedLobbyNumber;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("CONNECTED to internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("CONNECTED to the server");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("DISCONNECTED from the server");
    }

    public void OnConnectToPhotonServerButtonClick()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnDisconnectButtonClick()
    {
        PhotonNetwork.Disconnect();
    }

    public void OnCreateLobbyButtonClick(TMP_Text lobbyText)
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;

        LastCreatedLobbyNumber = Random.Range(1, 99999).ToString("D5");

        var lobbyOptions = new RoomOptions
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 2
        };

        PhotonNetwork.CreateRoom(LastCreatedLobbyNumber, lobbyOptions);
        UpdateLobbyText(lobbyText, LastCreatedLobbyNumber);
    }

    public void OnJoinLobbyButtonClick(int roomNumber)
    {
        PhotonNetwork.JoinRoom(roomNumber.ToString("D5"));
    }

    public void OnLeaveLobbyButtonClick(TMP_Text lobbyText)
    {
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

    private void UpdateLobbyText(TMP_Text lobbyText, string text)
    {
        if (lobbyText != null)
        {
            lobbyText.text = text;
        }
        else
        {
            Debug.LogError("Lobby text reference is not set.");
        }
    }
}
