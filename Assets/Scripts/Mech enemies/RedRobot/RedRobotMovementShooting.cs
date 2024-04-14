using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRobotMovementShooting : MonoBehaviour
{
    // Variables de uso general
    private GameObject hunter;
    private Rigidbody2D rb;
    private RedRobot r;

    // Variables para la persecusión
    [SerializeField] private bool mirandoDerecha = true;
    private float chaseSpeed = 1f;

    // Variables para el disparo
    private float distance;
    public GameObject bullet;
    public Transform firePoint;
    public AudioSource bulletSound;
    private float bulletCooldown = 0.1f;
    private float bulletShotInstant = 0f;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        r = GetComponent<RedRobot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!r.isDead())
        {
            // Lógica para el RedRobot: se ejecuta mientras no está muerto.
            // En todo momento sabe dónde está el cazador y camina hacia él. Se gira de manera acorde.
            // Si la distancia es menor o igual que X: dispara. En plan ametralladora, sin ráfagas.
            // Parte de animación
            if (hunter.transform.position.x <= transform.position.x && mirandoDerecha ||
                hunter.transform.position.x > transform.position.x && !mirandoDerecha)
            {
                Girar();
            }


            // Parte de movimiento
            if (hunter.transform.position.x <= transform.position.x)
            {
                rb.velocity = new Vector2(-1 * chaseSpeed, rb.velocity.y);
            }
            else if (hunter.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(1 * chaseSpeed, rb.velocity.y);
            }


            // Parte de disparos
            distance = Vector2.Distance(transform.position, hunter.transform.position);
            if (distance <= 10)
            {
                if (Time.time - bulletShotInstant >= bulletCooldown)
                {
                    ShootBullet();
                    bulletShotInstant = Time.time;
                }
            }
        }
    }

    private void ShootBullet()
    {
        bulletSound.Play();
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        transform.Rotate(0f, 180f, 0f);
    }
}
