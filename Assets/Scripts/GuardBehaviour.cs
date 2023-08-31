using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBehaviour : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform castPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float detectionRange;
    float wallDetectionDistance = 0.5f;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private LayerMask groundLayer;
    private bool onPatrol;
    private Animator anime;
    private Rigidbody2D rb;
    private LayerMask playerLayer;
    [SerializeField] Transform groundCheckPointLeft;
    [SerializeField] Transform groundCheckPointRight;
    private float groundCheckRadius = 0.2f;
    private SpriteRenderer sprite;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.GetMask("Player");
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if(PlayerSighted(detectionRange))
        {
            StartPursuit();
        }
        else
        {
            Patrol();
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if(!IsGrounded())
        {
            TurnAround();
        }

        if(onPatrol)
        {
            anime.SetBool("moving", true);
            anime.SetBool("pursue", false);
        }
        else if(!onPatrol)
        {
            anime.SetBool("moving", false);
            anime.SetBool("pursue", true);
        }

        if(rb.velocity.x < 0.0f)
        {
            sprite.flipX = false;
        }
        else if (rb.velocity.x > 0.0f)
        {
            sprite.flipX = true;
        }
    }

    private bool PlayerSighted(float distance)
    {
        bool isPlayerSighted = false;
        float castDistance = distance;

        // Set direction of ray cast depending on the direction the sprite is facing 
        Vector2 raycastDirection = sprite.flipX ? Vector2.right : Vector2.left;

        // 3D conversion of the 2d raycast 
        Vector3 raycastDirection3D = new Vector3(raycastDirection.x, raycastDirection.y, 0); 

        Vector3 endPosition = castPoint.position + raycastDirection3D * castDistance;

        // Draws the raycast 
        Debug.DrawLine(castPoint.position, endPosition, Color.red);

        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPosition, playerLayer);

        // Sets true when player is within line of sight and in detection range
        if(hit.collider != null && !WallCheck() && hit.collider.gameObject.CompareTag("Player"))
        {
            isPlayerSighted = true;
        }
        else
        {
            isPlayerSighted = false;
        }

        return isPlayerSighted;
    }

    // Guard pursues the player when in sight and increases in speed
    private void StartPursuit()
    {
        onPatrol = false;
        if(player.position.x < transform.position.x)
        {
            rb.velocity = new Vector2(-moveSpeed * 2, 0f);
        }
        else if(player.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(moveSpeed * 2, 0f);
        }
    }

    // Guard patrols area to look for player 
    private void Patrol()
    {
        onPatrol = true;

        if(sprite.flipX == false && !WallCheck() || sprite.flipX == true && WallCheck())
        {
            rb.velocity = new Vector2(-moveSpeed, 0f);
        }
        else if (sprite.flipX == false && WallCheck() || sprite.flipX == true && !WallCheck())
        {
            rb.velocity = new Vector2(moveSpeed, 0f);
        }
    }

    private void TurnAround()
    {
        if(rb.velocity.x > 0.0f)
        {
            rb.velocity = new Vector2(-moveSpeed, 0f);
        }
        else if(rb.velocity.x < 0.0f)
        {
            rb.velocity = new Vector2(moveSpeed, 0f);
        }
    }

    private bool WallCheck()
    {
        bool isWall = false;

        Vector2 raycastDirection = sprite.flipX ? Vector2.right : Vector2.left;
        Vector3 raycastDirection3D = new Vector3(raycastDirection.x, raycastDirection.y, 0); 
        Vector3 endPosition = castPoint.position + raycastDirection3D * wallDetectionDistance;

        RaycastHit2D wallHit = Physics2D.Linecast(castPoint.position, endPosition, LayerMask.GetMask("Ground"));
       
        if (wallHit.collider != null)
        {
            isWall = true;
        }

        return isWall;
    }
    private bool IsGrounded()
    {
        bool grounded = true;
        bool groundedLeft = Physics2D.OverlapCircle(groundCheckPointLeft.position, groundCheckRadius, groundLayer);
        bool groundedRight = Physics2D.OverlapCircle(groundCheckPointRight.position, groundCheckRadius, groundLayer);
        
        if(rb.velocity.x < 0f)
        {
            grounded = groundedLeft; 
        }
        else if(rb.velocity.x > 0f)
        {
            grounded = groundedRight;
        }

        Color debugColor =Color.green;
        Debug.DrawRay(groundCheckPointLeft.position, Vector2.down * groundCheckRadius, debugColor);
        Debug.DrawRay(groundCheckPointRight.position, Vector2.down * groundCheckRadius, debugColor);

        return grounded;
    }
}
