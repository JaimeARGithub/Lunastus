using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyRobotMovementShooting : MonoBehaviour
{
    // Variables de uso general
    private GameObject hunter;
    private Rigidbody2D rb;
    private GreyRobot g;

    // Variables para la persecusi�n
    [SerializeField] private bool mirandoDerecha = true;
    private float chaseSpeed = 1f;
    private bool hunterSeen = false;

    // Variables para el disparo
    private float distance;
    public GameObject bullet;
    public Transform firePoint;
    public AudioSource bulletSound;
    private float bulletCooldown = 1.5f;
    private float bulletShotInstant = 0f;



    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        g = GetComponent<GreyRobot>();
    }

    // Update is called once per frame
    void Update()
    {
        // L�gica para el GreyRobot: se ejecuta mientras no est� muerto.
        // En todo momento sabe d�nde est� el cazador y camina hacia �l. Se gira de manera acorde.
        // Si la distancia es menor o igual que X: dispara. Lanzamiento de misiles perseguidores, uno cada X tiempo.

        if (!g.isDead())
        {
            // Parte de animaci�n
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

            if (distance <= 15)
            {
                if (!hunterSeen)
                {
                    hunterSeen = true;
                    ShootBullet();
                }


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
