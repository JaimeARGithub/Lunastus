using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Referencia al jugador, para que las bolas de fuego sepan a dónde dirigirse
    private GameObject hunter;
    private Rigidbody2D rb;
    private float speed = 5f;
    private int damage = 25;
    public GameObject impactEffect;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hunter = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = hunter.transform.position - transform.position;
        // El .normalized es para asegurar que la dirección se mantenga siendo la misma
        rb.velocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Ajustamos el ángulo para que el sprite de la bola de fuego mire hacia el jugador
        transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.name.Contains("AlienBoss") && !collision.name.Contains("Bullet") && !collision.name.Equals("Missile(Clone)")
            && !collision.name.Contains("Item") && !collision.name.Contains("Limits") && !collision.name.Contains("Conversation"))
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                Health health = collision.GetComponent<Health>();
                if (health.isVulnerable())
                {
                    Debug.Log("DAÑO");
                    health.TakeDamage(damage);
                }
            }


            Vector3 impactPosition = transform.position;
            Vector3 bulletDirection = rb.velocity.normalized;
            impactPosition += bulletDirection * 0.5f;

            Instantiate(impactEffect, impactPosition, transform.rotation);
            Destroy(gameObject);
        }
    }
}
