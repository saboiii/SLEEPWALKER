using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    private GameObject player;
    private float directionMultiplier = 1f; // Multiplier to determine the projectile direction

    public GameObject destroyEffect;
    public float destroyDelay = 0.5f; // Delay in seconds before destroying the particle effect
    private GameObject effectInstance; // Reference to the instantiated particle effect

    public int damage = 1;
    private CameraShake cameraShake;

    void Start()
    {
        Invoke("DestroyEffect", destroyDelay); // Invoke the DestroyEffect function after the specified delay
        player = GameObject.FindGameObjectWithTag("Player");
        

        // Check if the player is facing left, and if so, multiply the direction by -1 to travel in the opposite direction
        if (player.transform.localScale.x < 0f)
        {
            directionMultiplier = -1f;
        }

        // Calculate the initial direction based on the projectile's rotation
        Vector3 direction = transform.right; // Use the projectile's right direction as the initial direction

        // Move the projectile in the calculated direction
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed * directionMultiplier;

        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void DestroyProjectile()
    {

        Destroy(gameObject);
    }

    void DestroyEffect()
    {
        effectInstance = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(effectInstance, destroyDelay); // Destroy the particle effect after the specified delay
        Invoke("DestroyProjectile", lifeTime);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (cameraShake != null)
        {
            cameraShake.ShakeOnHit();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                // Reduce the enemy's health using the damage value
                enemy.TakeDamage(damage);

            }
        }

        Invoke("DestroyEffect", 0.01f);
        
    }

}
