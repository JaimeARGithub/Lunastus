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


    private void Die()
    {
        Color colorSprite = spRd.material.color;
        colorSprite.a = 0f;
        spRd.material.color = colorSprite;

        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());

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
}
