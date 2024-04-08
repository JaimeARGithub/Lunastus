using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;
    private HealthBar healthBar;
    // public AudioSource healthUpgradeSound;
    // public AudioSource healSound;
    // public AudioSource takeDamageSound;
    // public AudioSource deathSound;
    private Animator animator;

    void Start()
    {
        maxHealth = 20; // CAMBIARLO PARA QUE EN EL START LEA DEL GAME MANAGER
        currentHealth = maxHealth; // CAMBIARLO PARA QUE EN EL START LEA DEL GAME MANAGER
        healthBar = FindObjectOfType<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // LEER TODO EL RATO VIDA ACTUAL Y VIDA MÁXIMA CON GETTERS DEL GAME MANAGER
    }

    public void upgradeHealth()
    {
        //healthUpgradeSound.Play();

        maxHealth += 25; // USAR SETTER DEL GAME MANAGER
        currentHealth = maxHealth; // USAR SETTER DEL GAME MANAGER (meter en el mismo método ampliar vida máxima e iguala valor de actual)
        healthBar.SetMaxHealth(maxHealth);
    }

    public void receiveHeal()
    {
        // healSound.Play();

        // USAR LOS SETTERS DEL GAME MANAGER
        if (currentHealth + 25 <= maxHealth)
        {
            currentHealth += 25;
        } else
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        //takeDamageSound.Play();

        // USAR LOS SETTERS DEL GAME MANAGER
        if (currentHealth - damage >= 0)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        } else
        {
            currentHealth = 0;
            healthBar.SetHealth(0);
        }


        if (currentHealth == 0)
        {
            animator.Play("HunterDeath");
            // deathSound.Play();
            Destroy(gameObject, 0.417f);
        }
    }
}
