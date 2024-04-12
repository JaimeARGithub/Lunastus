using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biceraptor : MonoBehaviour
{
    private int health = 140;
    public GameObject deathEffect;
    public AudioSource sonidoMuerte;
    private SpriteRenderer spRd;
    private bool dead = false;

    // Para el spawn de objetos al morir
    public GameObject healItem;
    public GameObject ammoItem;

    // Para la interacción con el movimiento, incluyendo marcarlo como dañado durante un tiempo
    private bool damaged = false;
    private float damageInstant = 0f;
    private float fleeTime = 2f; // Tiempo que se marca como dañado para que huya


    public void Start()
    {
        spRd = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (damaged && Time.time - damageInstant >= fleeTime)
        {
            damaged = false;
        }
    }


    public bool isDead()
    {
        return this.dead;
    }


    public void TakeDamage(int damage)
    {
        // El método de marcar como dañado se utiliza para que el script de movimiento lea sobre
        // la variable "dañado", para que el biceraptor sepa cuánto debe huir

        // En el script de movimiento, la variable "dañado" se lee en el update, con lo que la ejecución
        // es cada frame; para que la HUIDA se MANTENGA DURANTE UN TIEMPO, en el script de vida se va a
        // mantener marcado como DAÑADO durante UN TIEMPO
        damageInstant = Time.time;
        damaged = true;

        StartCoroutine(ChangeColor());
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }


    public bool isDamaged()
    {
        return this.damaged;
    }


    private void Die()
    {
        dead = true;
        // Al morir, al mismo tiempo se hacen invisible el objeto, se instancia la animación de muerte
        // y se reproduce el sonido de muerte; se eliminan el rigidbody y el collider
        Color colorSprite = spRd.material.color;
        colorSprite.a = 0f;
        spRd.material.color = colorSprite;

        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());

        SpawnItem();
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        sonidoMuerte.Play();

        // Tras emitirse el sonido de muerte con el objeto ya invisible y la animación de muerte
        // reproduciéndose, se destruye el objeto
        Destroy(gameObject, 0.7f);
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

    private void SpawnItem()
    {
        int random = Random.Range(1, 21); // Límite inferior incluido, límite superior excluido
        Debug.Log("ALEATORIO GENERADO: " + random);

        switch (random)
        {
            case 5:
                Instantiate(ammoItem, transform.position, Quaternion.identity);
                break;
            case 10:
            case 15:
            case 20:
                Instantiate(healItem, transform.position, Quaternion.identity);
                break;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health.isVulnerable())
            {
                Debug.Log("DAÑO");
                health.TakeDamage(10);
            }
        }
    }
}
