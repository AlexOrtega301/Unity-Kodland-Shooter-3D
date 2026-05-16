using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text ChatText;
    [SerializeField] TMP_InputField InputText;
    [SerializeField] TMP_Text PlayersText;
    [SerializeField] GameObject startButton;


    void Log(string message)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }
     public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // Mostrar un mensaje que informa que un jugador con un apodo específico ha abandonado la sala
        Log(otherPlayer.NickName + " Salió de la sala");
        RefreshPlayers();

        if (PhotonNetwork.IsMasterClient) // Si el jugador que abandonó la sala era el Cliente Maestro, el nuevo Cliente Maestro (el siguiente jugador en la lista) ahora tiene el control del botón de inicio
        {
            startButton.SetActive(true);
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        RefreshPlayers();
        // Mostrar un mensaje que informa que un jugador con un apodo específico ha ingresado a la sala
        Log(newPlayer.NickName + " entró a la sala");
    }

    //Metodos RPC son 
    [PunRPC]
    public void ShowMessage(string message)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }

    public void Send()
    {
        // Si el campo no tiene ningún texto, no hacemos nada
        if (string.IsNullOrWhiteSpace(InputText.text)) { 
            return; 
        }
        // Si un jugador presiona el botón Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Llamamos al método ShowMessage para todos los jugadores del servidor
            // Necesitamos generar el apodo del jugador y todo el texto que escribió en su campo de entrada
            photonView.RPC("ShowMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + InputText.text);
            // Borrar el string de texto en el campo de entrada
            InputText.text = string.Empty;
        }
    }

     void RefreshPlayers()
    {
        // La llamada sólo puede ser realizada por el Cliente Maestro (el jugador que creó el servidor)
        if (PhotonNetwork.IsMasterClient)
        {
            // Llamar al método ShowPlayers para todos los jugadores en el Lobby
            photonView.RPC("ShowPlayers", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ShowPlayers()
    {
        PlayersText.text = "Players: ";
        foreach (Photon.Realtime.Player otherPlayer in PhotonNetwork.PlayerList)
        {
            PlayersText.text += "\n";
            PlayersText.text += otherPlayer.NickName;
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }



    void Start()
    {
        RefreshPlayers();
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
