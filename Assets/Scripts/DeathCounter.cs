using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    private int deaths;
    [SerializeField] private Text DeathText;

    void Update(){
        deaths = PlayerPrefs.GetInt("Deaths");
        DeathText.text = "Deaths: " + deaths;

        // Manual method to reset death counter 
        PlayerPrefs.DeleteKey("Deaths");

    }

    private void OnCollisionEnter2D(Collision2D collision){

        if (collision.gameObject.tag == "Trap" || collision.gameObject.tag == "Enemy"){
            deaths++;
            PlayerPrefs.SetInt("Deaths", deaths);
            PlayerPrefs.Save();
        }
    }
}
