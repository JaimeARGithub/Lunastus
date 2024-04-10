using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelBullet : MonoBehaviour
{
    // Referencia al jugador, para que las balas sepan a dónde dirigirse
    private GameObject hunter;
    private Rigidbody2D rb;
    private float speed = 5f;
    public GameObject impactEffect;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hunter = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = hunter.transform.position - transform.position;
        // El .normalized es para asegurar que la dirección se mantenga siendo la misma
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;

        float rotation = Mathf.Atan2(-direction.x, -direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 180);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        if (!collision.name.Contains("Robot") && !collision.name.Contains("Bullet") && !collision.name.Equals("Missile(Clone)")
            && !collision.name.Contains("Item"))
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                Health health = collision.GetComponent<Health>();
                if (health.isVulnerable())
                {
                    Debug.Log("DAÑO");
                    health.TakeDamage(10);
                }
            }


            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
