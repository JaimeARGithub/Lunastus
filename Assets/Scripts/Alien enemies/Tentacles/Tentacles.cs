using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacles : MonoBehaviour
{
    private int health = 80;
    public GameObject deathEffect;
    public AudioSource deathSound;
    private SpriteRenderer spRd;

    // Para el spawn de objetos al morir
    public GameObject healItem;
    public GameObject ammoItem;

    void Start()
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
        Color colorSprite = spRd.material.color;
        colorSprite.a = 0f;
        spRd.material.color = colorSprite;

        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());

        SpawnItem();
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        deathSound.Play();

        Destroy(gameObject, 0.7f);
    }


    public IEnumerator ChangeColor()
    {
        spRd.color = Color.magenta;
        yield return new WaitForSeconds(0.1f);
        spRd.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }

    private void SpawnItem()
    {
        int random = Random.Range(1, 21); // L�mite inferior incluido, l�mite superior excluido
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
}