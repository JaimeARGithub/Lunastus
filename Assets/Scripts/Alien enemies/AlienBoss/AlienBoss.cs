using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBoss : MonoBehaviour
{
    private int health = 3500;
    public GameObject deathEffect;
    public AudioSource deathSound;
    private SpriteRenderer spRd;
    private bool dead = false;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        spRd = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }


    public bool isDead()
    {
        return this.dead;
    }


    public void TakeDamage(int damage)
    {
        StartCoroutine(ChangeColor());
        health -= damage;

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }


    private IEnumerator Die()
    {
        dead = true;
        // Al morir, al mismo tiempo se hacen invisible el objeto, se instancia la animación de muerte
        // y se reproduce el sonido de muerte; se eliminan el rigidbody y el collider
        Color colorSprite = spRd.material.color;
        colorSprite.a = 0f;
        spRd.material.color = colorSprite;

        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());


        Instantiate(deathEffect, transform.position, Quaternion.identity);
        deathSound.Play();

        // Tras emitirse el sonido de muerte con el objeto ya invisible y la animación de muerte
        // reproduciéndose, se destruye el objeto
        yield return new WaitForSeconds(3f);
        gameManager.BadEndingState();
        Destroy(gameObject);
    }


    public IEnumerator ChangeColor()
    {
        Color newColor = Color.red;
        float brightness = newColor.grayscale; // Obtener el brillo del color rojo
        newColor = newColor * (1.0f / brightness); // Ajustar el brillo
        spRd.color = newColor;
        yield return new WaitForSeconds(0.1f);
        spRd.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health.isVulnerable())
            {
                Debug.Log("DAÑO");
                health.TakeDamage(15);
            }
        }
    }
}
