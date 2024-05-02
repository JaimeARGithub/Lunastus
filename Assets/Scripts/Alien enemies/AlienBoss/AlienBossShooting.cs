using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBossShooting : MonoBehaviour
{
    // Variable de necesidad general
    private AlienBoss ab;

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



    // Start is called before the first frame update
    void Start()
    {
        ab = GetComponent<AlienBoss>();

        // Rugid inicial
        growlSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ab.isDead())
        {
            // Lógica de AlienBoss: actúa mientras no está muerto.
            // No se mueve ni se gira; PROCURAR QUE LAS DIMENSIONES DE LA SALA DE COMBATE CON RESPECTO A LAS DE ALIENBOSS NO PERMITAN QUE EL JUGADOR SE PONGA DETRÁS
            // La posición del jugador y la distancia no importan; el peligro es constante una vez se entra


            // Cada vez que acaba el cooldown de la fireball, lanza una
            if (Time.time - shootInstant >= shootCooldown)
            {
                ShootFireball();
                shootInstant = Time.time;
            }


            // Cada vez que acaba el cooldown del rugido, ruge otra vez
            if (Time.time - growlInstant >= growlCooldown)
            {
                growlSound.Play();
                growlInstant = Time.time;
            }
        }
    }


    private void ShootFireball()
    {
        fireSound.Play();
        Instantiate(fireball, firepoint.position, Quaternion.identity);
    }
}
