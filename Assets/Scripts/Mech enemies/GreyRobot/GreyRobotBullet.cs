using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class GreyRobotBullet : MonoBehaviour
{
    private float speed = 0.5f;
    private float distance;
    private int damage = 45;
    private Rigidbody2D rb;
    public GameObject impactEffect;

    // Para la persecución
    private GameObject hunter;
    private Vector2 direction;

    // Punto de empuje en el extremo derecho del misil
    public Transform pushPoint;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hunter = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Lógica del misil perseguidor: continuamente se conoce la posición del jugador
        // y se lo persigue despacio

        // Calcula la dirección hacia el jugador y la distancia con él
        direction = (hunter.transform.position - transform.position).normalized;

        // Aplica una fuerza en el punto de empuje
        rb.AddForceAtPosition(direction * speed, pushPoint.position);
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
                    health.TakeDamage(damage);
                }
            }


            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
