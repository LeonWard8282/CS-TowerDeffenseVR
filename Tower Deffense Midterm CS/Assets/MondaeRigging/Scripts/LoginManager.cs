using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerNameInput;

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region UI Callback Methods
    public void ConnectAnonymously()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ConnectToPhotonServer()
    {
        if(playerNameInput != null)
        {
            PhotonNetwork.NickName = playerNameInput.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #endregion

    #region Photon Callback Methods
    public override void OnConnected()
    {
        Debug.Log("OnConnectred called. Server available.");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conencted to Master Server with player name: " + PhotonNetwork.NickName);
        PhotonNetwork.LoadLevel("Home");
    }


    #endregion
}
