using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Variables related to movement and direction
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    bool isJumping = false;
    Rigidbody2D rb;
    bool isFacingRight;
    private Animator animator;

    // Variables related to effects
    public ParticleSystem dustJump;
    private CameraShake cameraShake;

    // Variables related to health
    public float health = 30;
    public float totalHealth;
    public Image healthBarFill;
    public GameObject deathOverlayPanel;

    // Variables related to score
    public int score = 0;
    public Text scoreText;
    public GameObject scoreHolder;
    public Text highscoreText;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isFacingRight = true;
        cameraShake = Camera.main.GetComponent<CameraShake>();
        totalHealth = health;
        scoreHolder.SetActive(true);
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        // Move left and right
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        if (Mathf.Abs(moveX) < 0.01f)
        {
            moveX = 0f;
        }
        rb.velocity = new Vector2(moveX, rb.velocity.y);


        // Jump
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
        }

        // Flip the player's direction
        if (moveX > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveX < 0 && isFacingRight)
        {
            Flip();
        }


        // Set animator parameters
        animator.SetFloat("Speed", Mathf.Abs(moveX));

        if (isJumping)
        {
            animator.SetBool("IsJumping", true);
        }
        else if (!isJumping)
        {
            animator.SetBool("IsJumping", false);
        }

        // Create jump/run effect
        if (Mathf.Abs(rb.velocity.x) > 0 && !isJumping)
        {
            CreateJumpDust();
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void CreateJumpDust()
    {
        dustJump.Play();
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float fillAmount = health / totalHealth;
            healthBarFill.fillAmount = fillAmount;
            Color fillColor = Color.Lerp(Color.red, Color.white, fillAmount);
            healthBarFill.color = fillColor;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            // Player defeated, perform any necessary actions
            Die();
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
    
    void Die()
    {
        deathOverlayPanel.SetActive(true);
        scoreHolder.SetActive(false);
        highscoreText.text = "Highscore: " + score.ToString();
        if (cameraShake != null)
        {
            cameraShake.ShakeOnPlayerDeath();
        }
        Destroy(gameObject);

    }

}
