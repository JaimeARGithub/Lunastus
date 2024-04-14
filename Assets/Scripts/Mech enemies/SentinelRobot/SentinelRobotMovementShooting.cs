using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelRobotMovementShooting : MonoBehaviour
{
    // Variables de necesidad general
    private GameObject hunter;
    private Sentinel s;

    // Variables para el movimiento
    [SerializeField] private bool mirandoDerecha = true;
    private float distance;

    // Variables para el disparo
    public GameObject bullet;
    public Transform bulletPosition1;
    public Transform bulletPosition2;
    public AudioSource bulletSound;

    private float shootInstant = 0f;
    private float shootCooldown = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        s = GetComponent<Sentinel>();
    }

    // Update is called once per frame
    void Update()
    {
        // L�gica del SentinelRobot: act�a mientras no est� muerto.
        // Si no est� muerto, continuamente se eval�a la distancia con el cazador.
        // Si �sta es igual o menor que 10, se inicia la secuencia de disparos y se gira el Sentinel Robot seg�n
        // la posici�n del cazador.
        if (!s.isDead())
        {
            distance = Vector2.Distance(transform.position, hunter.transform.position);

            if (distance <=10)
            {
                // Parte del giro del sprite
                if (hunter.transform.position.x < transform.position.x && mirandoDerecha ||
                    hunter.transform.position.x > transform.position.x && !mirandoDerecha)
                {
                    Girar();
                }


                // Parte de los disparos
                if (Time.time - shootInstant >= shootCooldown)
                {
                    Shoot();
                    shootInstant = Time.time;
                }
            }
        }
    }

    private void Shoot()
    {
        // El m�todo de disparo solamente genera la bala; el control de la orientaci�n se realiza en el script de la bala
        bulletSound.Play();
        Instantiate(bullet, bulletPosition1.position, Quaternion.identity);
        Instantiate(bullet, bulletPosition2.position, Quaternion.identity);
    }


    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        transform.Rotate(0f, 180f, 0f);
    }
}
