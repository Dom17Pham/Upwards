using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    private Animator anime;

    private bool levelCompleted = false;

    void Start()
    {
        anime = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.name == "Player"){
            anime.SetTrigger("finish");
            levelCompleted = true;
            Invoke("CompleteLevel", 3f);
        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
}
