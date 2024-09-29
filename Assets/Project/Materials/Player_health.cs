using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool isInvulnerable = false;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            currentHealth -= damage;
            Debug.Log($"Player took {damage} damage, current health: {currentHealth}");

            if (currentHealth > 0)
            {
                StartCoroutine(InvulnerabilityFrames());
            }
            else
            {
                Die();
            }
        }
    }

    private IEnumerator InvulnerabilityFrames()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(1.0f);
        isInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("Player died!");
    }
}
