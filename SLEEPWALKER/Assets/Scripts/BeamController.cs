using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeamController : MonoBehaviour
{
    public float damageAmount = 0.1f; // Amount of damage the beam inflicts on the player
    public GameObject player;
    public PlayerController playerController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {

    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the collided object is the player
            if (playerController != null)
            {
                // Call the TakeDamage method on the player's health script
                playerController.TakeDamage(damageAmount);
            }
        }
    }

}
