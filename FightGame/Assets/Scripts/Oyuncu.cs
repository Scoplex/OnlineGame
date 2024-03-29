using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Oyuncu : MonoBehaviour
{
    PhotonView pw;
    int saglik = 100;

    int canSayisi;

    sunucuYonetim sunucuyonetimi;

    int HedefOyuncu;

    private void Start()
    {
        canSayisi = 10;
        pw = GetComponent<PhotonView>();
        if (pw.IsMine)
        {
            GetComponent<Renderer>().material.color = Color.blue;

            if (PhotonNetwork.IsMasterClient) // Bu masayý kuran ben miyim demektir.
            {
                transform.position = new Vector3(-12.7f, 0, -13.3f);
                HedefOyuncu = 1;
                GameObject.FindWithTag("Oyuncu_1_Skor").GetComponent<TextMeshProUGUI>().text = canSayisi.ToString();
            }
            else
            {
                transform.position = new Vector3(13.6f, 0, 12.4f);
                HedefOyuncu = 0;
                GameObject.FindWithTag("Oyuncu_2_Skor").GetComponent<TextMeshProUGUI>().text = canSayisi.ToString();
            }

        }
    }
    void Update()
    {
        if (pw.IsMine)
        {
            hareket();
            zipla();
            atesEt();
        }

        // transform.Rotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0 * 500f * Time.deltaTime);
        
        if(Input.GetAxis("Mouse X") < 0)
        {
            transform.Rotate((Vector3.up) * -1.5f);
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            transform.Rotate((Vector3.up) * 1.5f);
        }
    }

    void atesEt()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position,transform.forward, out hit,100f))
            {
                if (hit.transform.gameObject.CompareTag("Dusman"))
                {
                    // hit.collider.gameObject.GetComponent<PhotonView>().RPC("darbeVer", RpcTarget.All, 20);
                    // rpc methodu herhangi bir scriptten herhangi bir fonksiyonu çaðýrmamýzý ve iþlem yapmamýzý saðlar.
                    // rpc ile çaðrýlan fonksiyonlarýn baþýnda [PunRPC] yazmasý gerekiyor.
                    hit.collider.gameObject.GetComponent<PhotonView>().RPC("CanAzalt", RpcTarget.All, HedefOyuncu);
                }
            }
        }
    }

    void hareket()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * 20f);
        transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * 20f);
    }
    void zipla()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 10, 0);
        }
    }

    [PunRPC]
    void darbeVer(int darbeGucu)
    {
        saglik -= darbeGucu;
        Debug.Log("Kalan Saðlýk: " + saglik);
        if(saglik <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    [PunRPC]
    void CanAzalt(int HedefOyuncu)
    {
        canSayisi--;
        GameObject.FindWithTag("SunucuYonetim").GetComponent<sunucuYonetim>().SkorGuncelle(HedefOyuncu, canSayisi);
       
    }
}
