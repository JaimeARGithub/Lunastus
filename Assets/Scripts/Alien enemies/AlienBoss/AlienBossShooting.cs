using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBossShooting : MonoBehaviour
{
    // Variable de necesidad general
    private GameObject hunter;
    private AlienBoss ab;

    // Variable para el movimiento
    [SerializeField] private bool mirandoDerecha = false;

    // Variables para lanzar bolas de fuego
    public Transform firepoint;
    public GameObject fireball;
    public AudioSource fireSound;
    private float shootInstant = 0f;
    private float shootCooldown = 1.5f;

    // Variables para el rugido cada X segundos
    public AudioSource growlSound;
    private float growlInstant = 0f;
    private float growlCooldown = 5f;

    // Variables para que no empiece a disparar hasta pasados tres segundos
    private bool startShooting = false;
    private float shootingStartDelay = 3f;
    private float firstInstant = 0f;



    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        ab = GetComponent<AlienBoss>();
        firstInstant = Time.time;

        // Rugid inicial
        growlSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ab.isDead())
        {
            // Lógica de AlienBoss: actúa mientras no está muerto.
            // Se gira dependiendo de la posición del jugador con respecto al boss.
            // La posición del jugador y la distancia no importan; el peligro es constante.
            // Una vez se entra, pasan 3 segundos hasta que el AlienBoss empieza a disparar.
            if (Time.time - firstInstant >= shootingStartDelay)
            {
                startShooting = true;
            }


            // Cada vez que acaba el cooldown de la fireball, dispara
            if (Time.time - shootInstant >= shootCooldown && startShooting)
            {
                ShootFireball();
                shootInstant = Time.time;
            }


            // Cada vez que acaba el cooldown del rugido, ruge
            if (Time.time - growlInstant >= growlCooldown)
            {
                growlSound.Play();
                growlInstant = Time.time;
            }


            // Parte de giro del sprite
            if (hunter.transform.position.x > transform.position.x && !mirandoDerecha ||
                hunter.transform.position.x < transform.position.x && mirandoDerecha)
            {
                Girar();
            }
        }
    }


    private void ShootFireball()
    {
        fireSound.Play();
        Instantiate(fireball, firepoint.position, Quaternion.identity);
    }


    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        transform.Rotate(0f, 180f, 0f);
    }
}
