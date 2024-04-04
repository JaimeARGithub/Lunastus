using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPosition1;
    public Transform bulletPosition2;
    public AudioSource bulletSound;

    private float bulletTimer = 0f;
    private float bulletCooldown = 0.5f;

    private Sentinel s;
    private GameObject hunter;


    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<Sentinel>();
        hunter = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Si el sentinel está muerto, no se hace nada.
        // Si está vivo, se calcula la distancia.
        // Si la distancia es menor de 10, se inicia la secuencia de disparos.

        if (!s.isDead())
        {

            float distance = Vector2.Distance(transform.position, hunter.transform.position);
            Debug.Log(distance);

            if (distance < 10)
            {
                bulletTimer += Time.deltaTime;

                if (bulletTimer >= bulletCooldown)
                {
                    bulletTimer = 0f;
                    Shoot();
                }
            }
        }
    }


    private void Shoot()
    {
        // El método de disparo solamente genera la bala; el control de la orientación se realiza en otro script
        bulletSound.Play();
        Instantiate(bullet, bulletPosition1.position, Quaternion.identity);
        Instantiate(bullet, bulletPosition2.position, Quaternion.identity);
    }
}
