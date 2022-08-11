using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class CrossbowDemonBehaviour : MonoBehaviour
{

    public ArrowProjectile ProjectilePrefab;
    public Transform LaunchOffset;
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

    }
}
