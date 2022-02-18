using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI playerNameText;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //Player is local player
            transform.GetComponent<Movement>().enabled = true;
            transform.GetComponent<Movement>().joystick.gameObject.SetActive(true);
        }
        else
        {
            //Player is remote player
            transform.GetComponent<Movement>().enabled = false;
            transform.GetComponent<Movement>().joystick.gameObject.SetActive(false);
        }
        SetPlayerName();
    }

    void SetPlayerName()
    {
        if (playerNameText != null)
        {
            if (photonView.IsMine)
            {
                playerNameText.text = "YOU";
                playerNameText.color = Color.red;
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;
            }
        }
    }
}
