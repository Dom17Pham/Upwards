using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBehaviour : MonoBehaviour
{
    private Animator anime;
    public EyeProjectile ProjectilePrefab;
    public HomingProjectile HomingProjectilePrefab;
    public Transform LaunchOffset;
    private BoxCollider2D box;

    private float CooldownTimer;
    private float Cooldown = 2;

    bool active = false;
    bool triggered = false;

    private Camera mainCamera;
    private Camera bossCamera;
    [SerializeField] private BoxCollider2D barrier;

    void Start()
    {
        anime = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        mainCamera = Camera.main;
        bossCamera = GameObject.Find("Eye Boss Camera")?.GetComponent<Camera>();
    }

    void Update()
    {
        if (active)
        {
            // Cooldown timer to set a time interval between each projectile
            // Prevents constant projectile generation ie infinite projectile firing.
            CooldownTimer -= Time.deltaTime;

            // Checks if the Cooldown time has been reached before resetting it
            if (CooldownTimer > 0) return;
            CooldownTimer = Cooldown;

            int randomNumber = Random.Range(1, 11); 
            bool isEven = randomNumber % 2 == 0;
            if (isEven)
            {
                Instantiate(HomingProjectilePrefab, LaunchOffset.position, transform.rotation);
            }
            else
            {
                Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
            }
        }
        else
        {
            if (!triggered) 
            {
                mainCamera.gameObject.SetActive(true);
                bossCamera.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;
            barrier.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            bossCamera.gameObject.SetActive(true);
            anime.SetBool("isPlayerHere", true);
        }
    }

    public void OnAnimationFinish()
    {
        active = true;
    }
}
