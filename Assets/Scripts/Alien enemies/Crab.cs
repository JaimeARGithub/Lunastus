using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    private int health = 80;
    public GameObject deathEffect;
    public AudioSource sonidoMuerte;
    private SpriteRenderer spRd;


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

        Instantiate(deathEffect, transform.position, Quaternion.identity);
        sonidoMuerte.Play();

        // Tras emitirse el sonido de muerte con el objeto ya invisible y la animación de muerte
        // reproduciéndose, se destruye el objeto
        Destroy(gameObject, 0.7f);
    }

    public IEnumerator ChangeColor()
    {
        spRd.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        spRd.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }
}
