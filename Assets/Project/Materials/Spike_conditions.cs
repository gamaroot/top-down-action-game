using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_conditions : MonoBehaviour
{
    //public int damage = 10;
    //public float damageCooldown = 1.0f;
    //private float lastDamageTime;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("You have been damaged");
            ApplyDamage(collision.gameObject);
            //Player_health playerHealth = collision.GetComponent<Player_health>();
            //if (playerHealth != null)
            //{
            // ApplyDamage(playerHealth);  
            //}
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Currently on damage frames");
            //Player_health playerHealth = collision.GetComponent<Player_health>();
            //if (playerHealth != null)
            //{
                // ApplyDamage(playerHealth);
                
            //}
        }
    }

    // Apply damage if enough time has passed since the last damage
    //private void ApplyDamage(Player_health playerHealth)
    //{
    //    if (Time.time - lastDamageTime >= damageCooldown)
    //    {
    //        playerHealth.TakeDamage(damage);
    //        lastDamageTime = Time.time; // Reset the damage timer
    //    }
    //}
    private void ApplyDamage(GameObject player)
    {
        
    }
}
