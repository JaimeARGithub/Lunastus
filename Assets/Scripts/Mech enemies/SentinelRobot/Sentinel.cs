using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentinel : MonoBehaviour
{
    private int health = 60;
    public GameObject deathEffect;
    public AudioSource idleSound;
    public AudioSource deathSound;
    private SpriteRenderer spRd;

    private bool dead = false;

    // Para el spawn de objetos al morir
    public GameObject healItem;
    public GameObject ammoItem;


    void Start()
    {
        spRd = GetComponent<SpriteRenderer>();
        idleSound.Play();
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(ChangeColor());
        health -= damage;

        if (health <= 0)
        {
            dead = true;
            Die();
        }
    }


    private void Die()
    {
        Color colorSprite = spRd.material.color;
        colorSprite.a = 0f;
        spRd.material.color = colorSprite;

        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());

        SpawnItem();
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        idleSound.Stop();
        deathSound.Play();

        Destroy(gameObject, 0.7f);
    }

    public IEnumerator ChangeColor()
    {
        spRd.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        spRd.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }


    public bool isDead()
    {
        return this.dead;
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
