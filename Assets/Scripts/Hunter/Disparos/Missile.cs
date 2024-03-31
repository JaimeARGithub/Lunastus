using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float velocidad = 15f;
    private Rigidbody2D rb;
    private int damageMissile = 150;
    public GameObject impactEffect;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * velocidad;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        if (!collision.name.Equals("Bullet(Clone)") && !collision.name.Equals("ChargedBullet(Clone)") && !collision.name.Equals("Missile(Clone)"))
        {
            switch (collision.name)
            {
                case "Crab":
                    Crab crab = collision.GetComponent<Crab>();
                    if (crab != null)
                    {
                        crab.TakeDamage(damageMissile);
                    }
                    break;
                case "Octopus":
                    Octopus octopus = collision.GetComponent<Octopus>();
                    if (octopus != null)
                    {
                        octopus.TakeDamage(damageMissile);
                    }
                    break;
                case "Jumper":
                    Jumper jumper = collision.GetComponent<Jumper>();
                    if (jumper != null)
                    {
                        jumper.TakeDamage(damageMissile);
                    }
                    break;
            }

            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
