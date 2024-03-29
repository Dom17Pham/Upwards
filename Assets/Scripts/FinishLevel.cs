using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{

    private bool levelCompleted;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.name == "Player"){
            levelCompleted = true;
            Invoke("CompleteLevel", 1f);
        }
        else
        {
            levelCompleted = false;
        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
}
