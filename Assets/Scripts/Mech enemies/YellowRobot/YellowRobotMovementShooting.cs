using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowRobotMovementShooting : MonoBehaviour
{
    // Variables de uso general
    private GameObject hunter;
    private Rigidbody2D rb;
    private YellowRobot y;

    // Variables para la persecusión
    [SerializeField] private bool mirandoDerecha = true;
    private float chaseSpeed = 3.5f;

    // Variables para el disparo
    private float distance;
    public GameObject bullet;
    public Transform firePoint;
    public AudioSource bulletSound;
    private float bulletCooldown = 0.15f;
    private float burstShotInstant = 0f;
    private float burstCooldown = 1f;



    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        y = GetComponent<YellowRobot>();
    }

    // Update is called once per frame
    void Update()
    {
        // Lógica para el YellowRobot: se ejecuta mientras no está muerto.
        // En todo momento sabe dónde está el cazador y camina hacia él. Se gira de manera acorde.
        // Si la distancia es menor o igual que X: dispara. Ráfagas de 2 disparos espaciados 0.15 segundos. 1 segundo de pausa entre ráfagas.
        if (!y.isDead())
        {
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
                if (Time.time - burstShotInstant >= burstCooldown)
                {
                    StartCoroutine(ShootBurst());
                    burstShotInstant = Time.time;
                }
            }
        }
    }

    private IEnumerator ShootBurst()
    {
        ShootBullet();
        yield return new WaitForSeconds(bulletCooldown);
        ShootBullet();
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
