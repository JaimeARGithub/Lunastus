using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    private int health = 40;
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
            Die();
        }
    }


    public bool isDead()
    {
        return this.dead;
    }


    private void Die()
    {
        dead = true;

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
        spRd.color = Color.green;
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
            case 3:
            case 6:
                Instantiate(ammoItem, transform.position, Quaternion.identity);
                break;
            case 13:
            case 16:
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
                health.TakeDamage(20);
            }
        }
    }
}
