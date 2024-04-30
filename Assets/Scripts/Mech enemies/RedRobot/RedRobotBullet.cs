using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRobotBullet : MonoBehaviour
{
    private float speed = 10f;
    private int damage = 30;
    private Rigidbody2D rb;
    public GameObject impactEffect;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        if (!collision.name.Contains("Robot") && !collision.name.Contains("Bullet") && !collision.name.Equals("Missile(Clone)")
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


            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
