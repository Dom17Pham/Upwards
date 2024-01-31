using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timers : MonoBehaviour
{
    [SerializeField] public TMP_Text InvisibilityTimeText;
    [SerializeField] public TMP_Text cooldownTimeText;

    private DavidMovement playerScript;

    private void Start()
    {
        playerScript = GetComponent<DavidMovement>();
    }


    void Update(){

        if (playerScript.transparencyTimer > 0f)
        {
            InvisibilityTimeText.text = "Invisibility Time: " + playerScript.transparencyTimer.ToString("0");
            playerScript.transparencyTimer -= Time.deltaTime;
        }
        else
        {
            InvisibilityTimeText.text = "";
        }

        if (playerScript.cooldownTimer > 0f)
        {
            cooldownTimeText.text = "Cooldown Time: " + playerScript.cooldownTimer.ToString("0");
            playerScript.cooldownTimer -= Time.deltaTime;
        }
        else
        {
            cooldownTimeText.text = "";
        }
    }

}
