using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    private int health = 60;
    public GameObject deathEffect;
    public AudioSource sonidoMuerte;
    private Renderer spRd;


    public void Start()
    {
        spRd = GetComponent<Renderer>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        // Al morir, al mismo tiempo se hacen invisible el objeto, se instancia la animación de muerte
        // y se reproduce el sonido de muerte
        Color colorSprite = spRd.material.color;
        colorSprite.a = 0f;
        spRd.material.color = colorSprite;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        sonidoMuerte.Play();

        // Tras emitirse el sonido de muerte con el objeto ya invisible y la animación de muerte
        // reproduciéndose, se destruye el objeto
        Destroy(gameObject, 0.7f);
    }
}
