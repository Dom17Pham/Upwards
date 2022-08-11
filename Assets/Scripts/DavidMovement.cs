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

    [SerializeField] private LayerMask JumpableGround;

    private float XDirect = 0f;
    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private float JumpForce = 12f;

    private enum MovementState { idle, running, jumping, falling}

    [SerializeField] private PhysicsMaterial2D FullFriction;
    [SerializeField] private PhysicsMaterial2D NormalFriction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anime = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        XDirect = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(XDirect * MoveSpeed, rb.velocity.y);

        // if the Jump button is pressed, player jumps
        if (Input.GetButtonDown("Jump") && GroundCheck())
        {
           rb.velocity = new Vector2(rb.velocity.x, JumpForce);
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
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

