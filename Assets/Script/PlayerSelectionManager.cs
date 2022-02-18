using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcherTransform;

    public Button next_Button;
    public Button prev_Button;

    public int PlayerSelectionNumber;

    public GameObject[] spinnerModels;

    public GameObject uI_Selection;
    public GameObject uI_AfterSelection;

    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;

    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);
        PlayerSelectionNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region UI Callback Methods
    public void NextPlayer()
    {
        PlayerSelectionNumber += 1;

        if (PlayerSelectionNumber >= spinnerModels.Length)
        {
            PlayerSelectionNumber = 0;
        }

        next_Button.enabled = false;
        prev_Button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));

        if (PlayerSelectionNumber == 0 || PlayerSelectionNumber == 1)
        {
            playerModelType_Text.text = "ATTACK";
        }
        else
        {
            playerModelType_Text.text = "DEFEND";
        }
    }
    public void PrevioudPlayer()
    {
        PlayerSelectionNumber -= 1;

        if (PlayerSelectionNumber < 0 )
        {
            PlayerSelectionNumber = spinnerModels.Length - 1;
        }

        next_Button.enabled = false;
        prev_Button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));

        if (PlayerSelectionNumber == 0 || PlayerSelectionNumber == 1)
        {
            playerModelType_Text.text = "ATTACK";
        }
        else
        {
            playerModelType_Text.text = "DEFEND";
        }
    }

    public void OnSelectButtonClicked()
    {
        uI_Selection.SetActive(false);
        uI_AfterSelection.SetActive(true);

        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { 
                                                                {MultiplayerSpinnerTopGame.PLAYER_SELECTION_NUMBER, 
                                                                 PlayerSelectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void OnReselectButtonClicked()
    {
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }
    #endregion

    #region Private Methods
    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {
        Quaternion originalRotation =  transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;

        next_Button.enabled = true;
        prev_Button.enabled = true;
    }
    #endregion
}
