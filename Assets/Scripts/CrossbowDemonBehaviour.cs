using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowDemonBehaviour : MonoBehaviour
{

    public ArrowProjectile ProjectilePrefab;
    public Transform LaunchOffset;

    public GameObject arrow;
    private SpriteRenderer sprite;
    void Update()
    {
        if(GameObject.Find("Arrow Projectile(Clone)") == null){
        Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);

        arrow = GameObject.Find("Arrow_0");
        sprite = arrow.GetComponent<SpriteRenderer>();
        sprite.sortingLayerID = SortingLayer.NameToID("Projectile");
        }
    }
}
