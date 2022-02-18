using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class BattleScript : MonoBehaviourPun
{
    public Spinner spinnerScript;

    public GameObject uI_3D_GameObject;
    public GameObject deathPanelUIPrefab;
    private GameObject deathPanelUIGameObject;
   
    private Rigidbody rb;

    private float startSpinSpeed;
    private float currentSpinSpeed;
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinSpeedRatio_Text;

    public float common_Damage_Coefficient = 0.04f;
    public bool isAttacker;
    public bool isDefender;
    private bool isDead = false;
    [Header("Player Type Damage")]
    public float doDamage_Coefficient_Attacker = 10f;
    public float getDamaged_Coefficient_Attacker = 1.2f;

    public float doDamage_Coefficient_Defender = 0.75f;
    public float getDamaged_Coefficient_Defender = 0.2f;


    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    private void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else if (gameObject.name.Contains("Defender"))
        {
            isAttacker = false;
            isDefender = true;

            spinnerScript.spinSpeed = 4400;

            startSpinSpeed = spinnerScript.spinSpeed;
            currentSpinSpeed = spinnerScript.spinSpeed;

            spinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            Debug.Log("My speed " + mySpeed + "----other player speed " + otherSpeed);

            if (mySpeed > otherSpeed)
            {
                Debug.Log("You damage other player");

                float default_Damage_Amount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600f * common_Damage_Coefficient;

                if (isAttacker)
                {
                    default_Damage_Amount *= doDamage_Coefficient_Attacker;
                }
                else if (isDefender)
                {
                    default_Damage_Amount *= doDamage_Coefficient_Defender;
                }

                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    //Apply Damage
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, default_Damage_Amount);
                }
            }
            
        }
    }

    [PunRPC]
    public void DoDamage(float _damageAmount)
    {
        if (!isDead)
        {
            if (isAttacker)
            {
                _damageAmount *= getDamaged_Coefficient_Attacker;
            }
            else if (isDefender)
            {
                _damageAmount *= getDamaged_Coefficient_Defender;
            }

            spinnerScript.spinSpeed -= _damageAmount;

            currentSpinSpeed = spinnerScript.spinSpeed;

            spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;

            spinSpeedRatio_Text.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed;

            if (currentSpinSpeed < 100)
            {
                //PLayer will die
                Die();
            }
        }
    }

    void Die()
    {

        isDead = true;

        GetComponent<Movement>().enabled = false;

        rb.freezeRotation = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        spinnerScript.spinSpeed = 0f;

        uI_3D_GameObject.SetActive(false);

        if (photonView.IsMine)
        {
            StartCoroutine(Respawn());
        }

    }

    IEnumerator Respawn()
    {
        GameObject canvasGameObject = GameObject.Find("Canvas");

        if (deathPanelUIGameObject == null)
        {
            deathPanelUIGameObject = Instantiate(deathPanelUIPrefab, canvasGameObject.transform);
        }
        else
        {
            deathPanelUIGameObject.SetActive(true);
        }

        Text respawnTimeText = deathPanelUIGameObject.transform.Find("RespawnTimeText").GetComponent<Text>();

        float respawnTime = 8.0f;

        respawnTimeText.text = respawnTime.ToString(".00");

        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);

            respawnTime -= 1.0f;
            respawnTimeText.text = respawnTime.ToString(".00");

            GetComponent<Movement>().enabled = false;

        }

        deathPanelUIGameObject.SetActive(false);

        GetComponent<Movement>().enabled = true;

        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }

    [PunRPC]

    public void Reborn()
    {
        spinnerScript.spinSpeed = startSpinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
        spinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;

        rb.freezeRotation = true;

        transform.rotation = Quaternion.Euler(Vector3.zero);

        uI_3D_GameObject.SetActive(true);

        isDead = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        CheckPlayerType();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
