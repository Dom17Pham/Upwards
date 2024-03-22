using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeProjectile : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotateSpeed = 200f;
    public int damage = 20;
    private GameObject Parent;
    private GameObject Child;
    private GameObject[] Parents;

    public Transform target;
    private Rigidbody2D rb;
    private GameObject[] player;

    Destructable barrier;

    Vector3 directionToPlayer;
    Vector3 targetPosition;

    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectsWithTag("Player");
        target = player[0].transform;
        directionToPlayer = (target.position - transform.position).normalized;
        targetPosition = target.position + directionToPlayer;
        // Creates a list of all arrow projectile in game scene
        if (Parents == null)
        {
            Parents = GameObject.FindGameObjectsWithTag("ParentProjectile");
        }

        // For each child of each projectile, change tag to projectile 
        // to differentiate it from its parent object and set child game object
        // to the same position as the parent.
        foreach (GameObject Parent in Parents)
        {
            if (Parent.transform.childCount > 0)
            {
                Child = Parent.transform.GetChild(0).gameObject;
                if (Child != null)
                {
                    Child.gameObject.tag = "Projectile";
                    Child.transform.localPosition = new Vector3(0f, 0f, 0f);
                }
            }
        }
    }
    void FixedUpdate()
    {
        //Vector3 directionToPlayer = (target.position - transform.position).normalized;
        //Vector3 targetPosition = target.position + directionToPlayer ;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Destructible"))
        {
            Destroy(gameObject);
            barrier = collision.gameObject.GetComponent<Destructable>();
            barrier.TakeDamage(damage);
        }
    }
}
