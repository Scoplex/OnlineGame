using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class sunucuYonetim : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Text serverBilgi;

    [SerializeField]
    InputField kulAdi, odaadi;

    string Nickname;
    string OdaAdi;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

        /* if (PhotonNetwork.IsConnected)
        {
            serverBilgi.text = "Server'a Baðlandý";
        } */

        DontDestroyOnLoad(gameObject);
    }

    public void odaKur()
    {
        SceneManager.LoadScene(1);
        Nickname = kulAdi.text;
        OdaAdi = odaadi.text;
        PhotonNetwork.JoinLobby();
    }
    public void girisYap()
    {
        SceneManager.LoadScene(1);
        Nickname = kulAdi.text;
        OdaAdi = odaadi.text;
        PhotonNetwork.JoinLobby();
    }

    public override void OnConnectedToMaster()
    {
        serverBilgi.text = "Server'a Baðlandý";
    }
    public override void OnJoinedLobby()
    {
        if (Nickname != "" && OdaAdi!= "")
        {
            PhotonNetwork.JoinOrCreateRoom(OdaAdi, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
    public override void OnJoinedRoom()
    {
        InvokeRepeating("isimVeBilgiKontrolEt", 0, 1f);
        GameObject objem = PhotonNetwork.Instantiate("Oyuncu", Vector3.zero, Quaternion.identity, 0, null);
        objem.GetComponent<PhotonView>().Owner.NickName = Nickname;
    }

    public override void OnLeftRoom()
    {
        // Odadan çýkýnca çalýþan fonksiyon.
    }
    public override void OnLeftLobby()
    {
        // Lobiden çýkýnca çalýþan fonksiyon.
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) // Kendim haricinde herhangi birisi oyundan çýkarsa, buraya girilir
    {
        InvokeRepeating("isimVeBilgiKontrolEt", 0, 1f);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // Herhangi bir odaya giremezse çýkacak olan hatadýr.
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // Rastgele bir odaya giremezse çýkacak olan hatadýr.
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // Oda oluþturm esnasýnda herhangi bir hata varsa çýkacak olan hatadýr.
    }

    void isimVeBilgiKontrolEt()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            GameObject.FindWithTag("OyuncuBekleniyor").GetComponent<TextMeshProUGUI>().text = "";
            GameObject.FindWithTag("Oyuncu_1").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Oyuncu_2").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName;
            CancelInvoke("isimVeBilgiKontrolEt");

        }
        else
        {
            //GameObject.Find("OyuncuBekleniyor").GetComponent<TextMeshProUGUI>().text = "Oyuncu Bekleniyor..";
            GameObject.FindWithTag("OyuncuBekleniyor").GetComponent<TextMeshProUGUI>().text = "Oyuncu Bekleniyor..";
            GameObject.FindWithTag("Oyuncu_1").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Oyuncu_2").GetComponent<TextMeshProUGUI>().text = ".....";
        }
    }
    public void SkorGuncelle(int oyuncuSirasi, int SayiNedir)
    {
        switch(oyuncuSirasi)
        {
            case 0:
                GameObject.FindWithTag("Oyuncu_1_Skor").GetComponent<TextMeshProUGUI>().text = SayiNedir.ToString();
            break;
            case 1:
                GameObject.FindWithTag("Oyuncu_2_Skor").GetComponent<TextMeshProUGUI>().text = SayiNedir.ToString();
            break;
        }

        if(SayiNedir <=0)
        {
            if (oyuncuSirasi==0)
            {
                foreach (var objem in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                {
                    if (objem.gameObject.CompareTag("WinPanel"))
                    {
                        objem.gameObject.SetActive(true);
                        GameObject.FindWithTag("KazananKisi").GetComponent<TextMeshProUGUI>().text = "2. Oyuncu Yendi hahaha";
                    }
                }
            }
            else
            {
                foreach (var objem in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                {
                    if (objem.gameObject.CompareTag("WinPanel"))
                    {
                        objem.gameObject.SetActive(true);
                        GameObject.FindWithTag("KazananKisi").GetComponent<TextMeshProUGUI>().text = "1. Oyuncu Yendi hahaha";
                    }
                }
            }
        }
    }

}
