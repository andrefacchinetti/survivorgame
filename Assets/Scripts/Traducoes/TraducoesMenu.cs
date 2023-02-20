using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TraducoesMenu : MonoBehaviour
{

    public TMP_Text LOADING, Settings, Rooms, PLAY, Rules, Leaderboard, BACK; //TitleMenu
    public TMP_Text InsertRoomName, Enterapasswordorkeepempty, MaxPlayers, CreateRoom; //CreateRoomMenu
    public TMP_Text StartGame, Leave, Digite; //RoomMenu
    public TMP_Text Settings2, Back2; //Settings
    public TMP_Text FindRoom, CreateRoom2, Back, Insertroomname; //FindRoomMenu
    public TMP_Text Leaderboard2, Global, Friends; //LeaderboardMenu
    public TMP_Text lookingforopponents, Cancel; //FindMatchmaking

    // Start is called before the first frame update
    void Awake()
    {
        CarregarTraducoes();
    }

    public void CarregarTraducoes()
    {
        if (PlayerPrefs.GetInt("INDEXIDIOMA") == 1) //PORTUGUES
        {
            if (LOADING != null) LOADING.text = "CARREGANDO...";
            if (Settings != null) Settings.text = "Configurações";
            if (Rooms != null) Rooms.text = "Salas";
            if (PLAY != null) PLAY.text = "Jogar";
            if (Rules != null) Rules.text = "Manual";
            if (Leaderboard != null) Leaderboard.text = "Placar";
            if (BACK != null) BACK.text = "Voltar";
            if (Back2 != null) Back2.text = "Voltar";

            if (InsertRoomName != null) InsertRoomName.text = "Insira o nome da sala...";
            if (Enterapasswordorkeepempty != null) Enterapasswordorkeepempty.text = "Insira uma senha ou deixe em branco...";
            if (MaxPlayers != null) MaxPlayers.text = "Máximo de Jogadores";
            if (CreateRoom != null) CreateRoom.text = "Criar Sala";

            if (StartGame != null) StartGame.text = "Iniciar Jogo";
            if (Leave != null) Leave.text = "Sair";
            if (Digite != null) Digite.text = "Digite...";

            if (FindRoom != null) FindRoom.text = "Encontrar Sala";
            if (CreateRoom2 != null) CreateRoom2.text = "Criar Sala";
            if (Back != null) Back.text = "Voltar";
            if (Insertroomname != null) Insertroomname.text = "Insira o nome da sala...";

            if (Settings2 != null) Settings2.text = "Configurações";
            if (Leaderboard2 != null) Leaderboard2.text = "Placar";

            if (lookingforopponents != null) lookingforopponents.text = "Procurando oponentes...";
            if (Cancel != null) Cancel.text = "Cancelar";

            if (Global != null) Global.text = "Global";
            if (Friends != null) Friends.text = "Amigos";
        }
        else //INGLES
        {
            if (LOADING != null) LOADING.text = "LOADING...";
            if (Settings != null) Settings.text = "Settings";
            if (Rooms != null) Rooms.text = "Room's";
            if (PLAY != null) PLAY.text = "Play";
            if (Rules != null) Rules.text = "Rules";
            if (Leaderboard != null) Leaderboard.text = "Leaderboard";
            if (BACK != null) BACK.text = "Back";
            if (Back2 != null) Back2.text = "Back";

            if (InsertRoomName != null) InsertRoomName.text = "Insert Room Name...";
            if (Enterapasswordorkeepempty != null) Enterapasswordorkeepempty.text = "Enter a password or keep empty...";
            if (MaxPlayers != null) MaxPlayers.text = "Max Players";
            if (CreateRoom != null) CreateRoom.text = "Create Room";

            if (StartGame != null) StartGame.text = "Start Game";
            if (Leave != null) Leave.text = "Leave";
            if (Digite != null) Digite.text = "Type it...";

            if (FindRoom != null) FindRoom.text = "Find Room";
            if (CreateRoom2 != null) CreateRoom2.text = "Create Room";
            if (Back != null) Back.text = "Back";
            if (Insertroomname != null) Insertroomname.text = "Insert room name";

            if (Settings2 != null) Settings2.text = "Settings";
            if (Leaderboard2 != null) Leaderboard2.text = "Leaderboard";

            if (lookingforopponents != null) lookingforopponents.text = "looking for opponents...";
            if (Cancel != null) Cancel.text = "Cancel";

            if (Global != null) Global.text = "Global";
            if (Friends != null) Friends.text = "Friends";
        }
    }
   
}
