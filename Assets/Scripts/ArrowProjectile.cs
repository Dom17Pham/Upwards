using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float speed = 5.0f;
    private GameObject Parent;
    private GameObject Child;
    private GameObject[] Parents;

    void Start()
    {
        // Creates a list of all arrow projectile in game scene
        if (Parents == null){
            Parents = GameObject.FindGameObjectsWithTag("ParentProjectile");
        }

        // For each child of each arrow projectile, change tag to projectile 
        // to differentiate it from its parent object and set child game object
        // to the same position as the parent.
        foreach(GameObject Parent in Parents){
            Child = Parent.transform.GetChild(0).gameObject;
            Child.gameObject.tag = "Projectile";
            Child.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
    void Update()
    {
        // Moves the arrow projectile to the right 
        transform.position += -transform.right * Time.deltaTime * speed;
    }

    // Method below is used to destroy the projectile when it collides 
    // with another 2d object or with the terrain
    private void OnCollisionEnter2D(Collision2D collision){
        Destroy(gameObject);
    }
}
