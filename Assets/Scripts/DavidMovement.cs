using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DavidMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anime;

    MovementState state;

    private float XDirect = 0f;

    // Movement properties
    private enum MovementState { idle, running, jumping, falling}
    [SerializeField] private LayerMask JumpableGround;
    [SerializeField] private LayerMask JumpableWall;
    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private float JumpForce = 12f;

    [SerializeField] private PhysicsMaterial2D FullFriction;
    [SerializeField] private PhysicsMaterial2D NormalFriction;

    private bool isTransparent = false;
    private Color originalColor;
    private float transparencyDuration = 10f;  
    private float cooldownDuration = 30f;      
    public float transparencyTimer = 0f;
    public float cooldownTimer = 0f;
    private bool isTransparencyTimerExpired = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anime = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        
        originalColor = sprite.color;
    }

    void Update()
    {
        XDirect = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(XDirect * MoveSpeed, rb.velocity.y);

        // if the Jump button(Spacebar) is pressed, player jumps
        if (Input.GetButtonDown("Jump") && GroundCheck())
        {
           rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }   

        // Check for Ctrl key press to toggle transparency
        if (Input.GetKeyDown(KeyCode.LeftControl) && cooldownTimer <= 0f)
        {
            isTransparent = !isTransparent;
            UpdateTransparency();
            // Start the transparency timer if becoming transparent
            if (isTransparent)
            {
                transparencyTimer = transparencyDuration;
            }
            else
            {
                transparencyTimer = 0f;
            }
        }
        
        // Decrement the timers
        if (transparencyTimer > 0f && isTransparent)
        {
            transparencyTimer -= Time.deltaTime;
        }
        if(cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Check if the transparency timer has expired
        if (transparencyTimer <= 0f && isTransparent && !isTransparencyTimerExpired)
        {
            isTransparencyTimerExpired = true;
            isTransparent = false;
            UpdateTransparency();
        }

        if(isTransparencyTimerExpired)
        {
            isTransparencyTimerExpired = false;
            cooldownTimer = cooldownDuration;
        }

        // Change the player's tag to "Invisible" when becoming transparent
        if (isTransparent)
        {
            gameObject.tag = "Invisible";
        }
        // Change the player's tag back to "Player" when not transparent
        else
        {
            gameObject.tag = "Player";
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
    
        //MovementState state;
        rb.sharedMaterial = NormalFriction;

        // Animate running when player moving to the right
        if (XDirect > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        // Animate running when player moving to the left
        else if (XDirect < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        // Animate idle when player is not moving 
        else
        {
            state = MovementState.idle;
            rb.sharedMaterial = FullFriction;
        }

        if (rb.velocity.y > .1f && !GroundCheck())
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f && !GroundCheck())
        {
            state = MovementState.falling;
        }

        anime.SetInteger("state", (int)state);
    }

    private bool GroundCheck()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, JumpableGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Trap" || collision.gameObject.tag == "ParentProjectile")
        {
            anime.SetTrigger("death");
            rb.simulated = false;
        }
    }

    private void UpdateTransparency()
    {
        if (isTransparent)
        {
            Color transparentColor = originalColor;
            transparentColor.a = 0.25f; // Adjust this value for transparency level
            sprite.color = transparentColor;
        }
        else
        {
            sprite.color = originalColor;
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

