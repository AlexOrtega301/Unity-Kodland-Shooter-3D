using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class MenuManager : MonoBehaviourPunCallbacks
{

    [SerializeField] TMP_Text logText;
    [SerializeField] TMP_InputField inputField;

    public void ChangeName()
    {
        //Leer el texto que el jugador ha escrito en el InputField
        PhotonNetwork.NickName = inputField.text;
        //Generando el nuevo apodo
        Log("New Player name: " + PhotonNetwork.NickName);
    }

    void Log(string message)
    {
        // Moviendo texto a la línea siguiente
        logText.text += "\n";
        // Agregando un mensaje
        logText.text += message;
    }

    void Start()
    {
        // Dando a un jugador un apodo con un número aleatorio
        PhotonNetwork.NickName = "Player" + Random.Range(1, 9999);
        // Mostrar el apodo en el campo Log
        Log("Player Name: " + PhotonNetwork.NickName);
        // Configurando el juego
        PhotonNetwork.AutomaticallySyncScene = true; // Automatically switching between windows
        PhotonNetwork.GameVersion = "1"; // Setting the game's version
        PhotonNetwork.ConnectUsingSettings(); // Connecting to the Photon server
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 15 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        Log("Connected to the server");
    }

    public override void OnJoinedRoom()
    {
        Log("Joined the lobby");
        PhotonNetwork.LoadLevel("Lobby");
    }

    void Update()
    {
        
    }
}
