using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0,0, 360 * speed * Time.deltaTime); 
    }
}
