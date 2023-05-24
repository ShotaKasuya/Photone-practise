using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    string GameVersion = "Ver1.0";
    [SerializeField] private cameraController cameraControl;
    [SerializeField] private Transform[] Pos;
    [SerializeField] private GameObject Canbas;
    [SerializeField] private Text game_end_text;

    static RoomOptions RoomOPS = new RoomOptions() {
        MaxPlayers = 2,
        IsOpen = true,
        IsVisible = true,
    };

    void Start() {
        Canbas.SetActive(false);
        Debug.Log("Photon Login");
        PhotonNetwork.GameVersion = GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Photon", RoomOPS, null);
    }

    public override void OnJoinedRoom()
    {
        Room myroom = PhotonNetwork.CurrentRoom;
        Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;
        Debug.Log("room name : " + myroom.Name);
        Debug.Log("Player No : " + player.ActorNumber);
        Debug.Log("Player ID : " + player.UserId);

        player.NickName = "No." + player.ActorNumber.ToString();

        Debug.Log("Player Name : " + player.NickName);
        Debug.Log("Room Master : " + player.IsMasterClient);

        GameObject UserAbator = PhotonNetwork.Instantiate("Player", Pos[player.ActorNumber - 1].position, Quaternion.identity,0);
        PlayerMove playerMove = UserAbator.GetComponent<PlayerMove>();
        cameraControl.set_player(UserAbator.transform);
        playerMove.PlayerNumber = player.ActorNumber;
        playerMove.login = this;
        playerMove.enabled = true;
    }

    public void game_set(int i)
    {
        Canbas.SetActive(true);
        game_end_text.text = "ゲーム終了\nプレーヤー" + i.ToString() + "の負け";
        Time.timeScale = 0;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Login Failued");
        PhotonNetwork.CreateRoom(null, RoomOPS);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed");
    }

    // // データの送受信を行うメソッド
    // // sendNext()とReceiveNext()の引数となる動機対象となるデータは順番を整える必要がある
    // // sendNext()はストリームブロックに動機対象データを書き込む
    // // ReceiveNext()はストリームブロックに書き込まれているデータを読み取り次のブロックに進む
    // void IPunObservable.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info) {
    //     // 処理対象が自身のオブジェクトである場合
    //     if (stream.IsWriting) {
    //         // オブジェクトの状態を管理するデータをストリームに送る
    //         stream.SendNext();
    //     } else {
    //         // 1. オブジェクトの状態を管理するデータをストリームから受け取る
    //     }
    // }
}
