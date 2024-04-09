using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;
    private HealthBar healthBar;
    public AudioSource healthUpgradeSound;
    public AudioSource healSound;
    public AudioSource damageSound;
    public GameObject deathAnimation;

    private SpriteRenderer spRd;
    private bool vulnerable = true;
    private float invulnerableInstant = 0f;
    private float invulnerabilityTime = 1.5f;

    void Start()
    {
        maxHealth = 100; // CAMBIARLO PARA QUE EN EL START LEA DEL GAME MANAGER
        currentHealth = maxHealth; // CAMBIARLO PARA QUE EN EL START LEA DEL GAME MANAGER
        healthBar = FindObjectOfType<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);

        spRd = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // LEER TODO EL RATO VIDA ACTUAL Y VIDA M�XIMA CON GETTERS DEL GAME MANAGER

        if (!vulnerable && Time.time - invulnerableInstant >= invulnerabilityTime)
        {
            vulnerable = true;

            Color colorSprite = spRd.material.color;
            colorSprite.a = 1f;
            spRd.material.color = colorSprite;
        }
    }

    public void upgradeHealth()
    {
        healthUpgradeSound.Play();

        maxHealth += 25; // USAR SETTER DEL GAME MANAGER
        currentHealth = maxHealth; // USAR SETTER DEL GAME MANAGER (meter en el mismo m�todo ampliar vida m�xima e iguala valor de actual)
        healthBar.SetMaxHealth(maxHealth);
    }

    public void receiveHeal()
    {
        healSound.Play();

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
        damageSound.Play();
        makeInvulnerable();


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


        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    public bool isVulnerable()
    {
        return this.vulnerable;
    }


    private void makeInvulnerable()
    {
        vulnerable = false;
        invulnerableInstant = Time.time;

        Color colorSprite = spRd.material.color;
        colorSprite.a = 0.75f;
        spRd.material.color = colorSprite;
    }
}