using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Variables related to health
    public int health = 10; // Hash of the HurtTrigger parameter

    //Variables related to movement
    public float speed = 2f;
    private Rigidbody2D rb;
    public PlayerController playerController;
    public Transform player;
    private bool isMovementStopped = false;
    bool isFacingLeft;
    public float raycastDistance = 1f;
    public float beamPreparationDistance = 5;
    private bool damageCooldown = false;
    public string hurtTrigger = "HurtTrigger";

    //Variables related to animation/visuals
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.green;
    public float hitColorDuration = 0.5f;
    public GameObject beamObject; // Reference to the beam
    public GameObject eye;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
        beamObject = transform.Find("BeamObject").gameObject;
        eye = transform.Find("Eye").gameObject;
        isFacingLeft = true;
    }

    void Update()
    {
        if (player == null)
        {
            // Player reference is null, do nothing
            return;
        }
        else
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer > 2)
            {
                if (distanceToPlayer <= beamPreparationDistance)
                {
                    // Enemy is within the desired range to prepare the beam
                    StopMovement();
                    if (isMovementStopped)
                    {
                        beamObject.SetActive(true);
                        UpdateBeamPosition();
                        StartCoroutine(ResumeMovementAfterDelay(5f));
                        return;
                    }
                }
                else
                {
                    // Enemy is outside the range, continue moving towards the player
                    ResumeMovement(direction);
                    beamObject.SetActive(false);
                }
            }

            else if (distanceToPlayer < 2)
            {
                MoveAwayFromPlayer(direction);
            }
        }
        
    }

    private void MoveHorizontally(Vector2 direction)
    {
        // Calculate the horizontal direction to move
        float horizontalDirection = Mathf.Sign(direction.x);

        if (horizontalDirection < 0 && !isFacingLeft)
        {
            Flip();
        }
        else if (horizontalDirection > 0 && isFacingLeft)
        {
            Flip();
        }


        rb.velocity = new Vector2(speed * horizontalDirection, rb.velocity.y);
    }

    private void MoveAwayFromPlayer(Vector2 direction)
    {
        // Calculate the horizontal direction to move away from the player
        float horizontalDirection = Mathf.Sign(direction.x) * -1f;
        rb.velocity = new Vector2(speed * horizontalDirection, rb.velocity.y);
    }

    private void StopMovement()
    {
        rb.velocity = Vector2.zero; // Stop the enemy's movement by setting its velocity to zero
        isMovementStopped = true;
    }

    private void ResumeMovement(Vector2 direction)
    {
        if (damageCooldown)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        MoveHorizontally(direction);
        MoveVertically(direction);

    }

    private IEnumerator ResumeMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (player == null)
            yield break;

        Vector2 direction = (player.position - transform.position).normalized;
        ResumeMovement(direction);
        isMovementStopped = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            // Enemy defeated
            Destroy(gameObject);
        }
        else
        {
            playerController.IncreaseScore(5);
            StartCoroutine(Cooldown(1f));

            // Play the hurt animation
            if (animator != null)
            {
                animator.SetTrigger(hurtTrigger);
            }

            // Apply the hit color
            if (spriteRenderer != null)
            {
                StartCoroutine(ApplyHitColor());
            }

        }
    }

    private IEnumerator Cooldown(float duration)
    {
        damageCooldown = true;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(duration);

        damageCooldown = false;
    }

    

    private void MoveVertically(Vector2 direction)
    {
        // Calculate the vertical direction to move
        float verticalDirection = Mathf.Sign(direction.y);

        // No obstacle checking for simplicity
        rb.velocity = new Vector2(rb.velocity.x, speed * verticalDirection);
    }


    private void Flip()
    {
        isFacingLeft = !isFacingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

    }


    private void UpdateBeamPosition()
    {
        if (beamObject != null)
        {
            // Set the position of the beamObject to match the enemy's position
            beamObject.transform.position = eye.transform.position;
        }
    }

    private IEnumerator ApplyHitColor()
    {
        Color startColor = Color.white;
        Color hitColor = Color.green;
        float transitionDuration = 0.5f;

        // Transition to hit color
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, hitColor, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Transition back to original color
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            spriteRenderer.color = Color.Lerp(hitColor, startColor, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = startColor;
    }

}
