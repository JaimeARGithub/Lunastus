using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownRobot : MonoBehaviour
{
    private int health = 40;
    public GameObject deathEffect;
    public AudioSource sonidoMuerte;
    private SpriteRenderer spRd;

    // Para el spawn de objetos al morir
    public GameObject healItem;
    public GameObject ammoItem;


    public void Start()
    {
        spRd = GetComponent<SpriteRenderer>();
    }


    public void TakeDamage(int damage)
    {
        StartCoroutine(ChangeColor());
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
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
        spRd.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spRd.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }

    private void SpawnItem()
    {
        int random = Random.Range(1, 11); // Límite inferior incluido, límite superior excluido
        Debug.Log("ALEATORIO GENERADO: " + random);

        switch (random)
        {
            case 10:
                Instantiate(healItem, transform.position, Quaternion.identity);
                break;
            case 9:
                Instantiate(ammoItem, transform.position, Quaternion.identity);
                break;
            case 8:
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
                health.TakeDamage(5);
            }
        }
    }
}
