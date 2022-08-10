using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class CrossbowDemonBehaviour : MonoBehaviour
{

    public ArrowProjectile ProjectilePrefab;
    public Transform LaunchOffset;
    public GameObject Arrow;
    private SpriteRenderer sprite;
    private float CooldownTimer;
    private float Cooldown = 3;
    void Update()
    {
        // Cooldown timer to set a time interval between each projectile
        // Prevents constant projectile generation ie infinite projectile firing.
        CooldownTimer -= Time.deltaTime;

        // Checks if the Cooldown time has been reached before resetting it
        if (CooldownTimer > 0) return;
        CooldownTimer = Cooldown;

        // Create a new projectile game object 
        Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        
        // Arrow is set to the child game object of the arrow projectile created above
        Arrow = GameObject.Find("Arrow_0");
        // Arrow child game object is moved to the same position as parent
        Arrow.transform.localPosition = new Vector3(0, 0, 0);
    }
}
