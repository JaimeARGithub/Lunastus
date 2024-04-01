using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float velocidad = 15f;
    private Rigidbody2D rb;
    private int damageBullet = 20;
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

        if (!collision.name.Equals("Hunter"))
        {
            if (!collision.name.Equals("Bullet(Clone)") && !collision.name.Equals("ChargedBullet(Clone)") && !collision.name.Equals("Missile(Clone)"))
            {
                switch (collision.name)
                {
                    case "Crab":
                        Crab crab = collision.GetComponent<Crab>();
                        if (crab != null)
                        {
                            crab.TakeDamage(damageBullet);
                        }
                        break;
                    case "Octopus":
                        Octopus octopus = collision.GetComponent<Octopus>();
                        if (octopus != null)
                        {
                            octopus.TakeDamage(damageBullet);
                        }
                        break;
                    case "Jumper":
                        Jumper jumper = collision.GetComponent<Jumper>();
                        if (jumper != null)
                        {
                            jumper.TakeDamage(damageBullet);
                        }
                        break;
                    case "Fly":
                        Fly fly = collision.GetComponent<Fly>();
                        if (fly != null)
                        {
                            fly.TakeDamage(damageBullet);
                        }
                        break;
                    case "Eye":
                        Eye eye = collision.GetComponent<Eye>();
                        if (eye != null)
                        {
                            eye.TakeDamage(damageBullet);
                        }
                        break;
                    case "Rinofish":
                        Rinofish rinofish = collision.GetComponent<Rinofish>();
                        if (rinofish != null)
                        {
                            rinofish.TakeDamage(damageBullet);
                        }
                        break;
                    case "Tentacles":
                        Tentacles tentacles = collision.GetComponent<Tentacles>();
                        if (tentacles != null)
                        {
                            tentacles.TakeDamage(damageBullet);
                        }
                        break;
                    case "Shell":
                        Shell shell = collision.GetComponent<Shell>();
                        if (shell != null)
                        {
                            shell.TakeDamage(damageBullet);
                        }
                        break;
                    case "Biceraptor":
                        Biceraptor biceraptor = collision.GetComponent<Biceraptor>();
                        if (biceraptor != null)
                        {
                            biceraptor.TakeDamage(damageBullet);
                        }
                        break;
                    case "Sentinel":
                        Sentinel sentinel = collision.GetComponent<Sentinel>();
                        if (sentinel != null)
                        {
                            sentinel.TakeDamage(damageBullet);
                        }
                        break;
                    case "YellowRobot":
                        YellowRobot yellowRobot = collision.GetComponent<YellowRobot>();
                        if (yellowRobot != null)
                        {
                            yellowRobot.TakeDamage(damageBullet);
                        }
                        break;
                    case "GreyRobot":
                        GreyRobot greyRobot = collision.GetComponent<GreyRobot>();
                        if (greyRobot != null)
                        {
                            greyRobot.TakeDamage(damageBullet);
                        }
                        break;
                    case "BrownRobot":
                        BrownRobot brownRobot = collision.GetComponent<BrownRobot>();
                        if (brownRobot != null)
                        {
                            brownRobot.TakeDamage(damageBullet);
                        }
                        break;
                    case "RedRobot":
                        RedRobot redRobot = collision.GetComponent<RedRobot>();
                        if (redRobot != null)
                        {
                            redRobot.TakeDamage(damageBullet);
                        }
                        break;
                    case "Gate":
                        Gate gate = collision.GetComponent<Gate>();
                        if (gate != null)
                        {
                            gate.TakeDamage(damageBullet);
                        }
                        break;
                    case "MissileGate":
                        MissileGate missileGate = collision.GetComponent<MissileGate>();
                        if (missileGate != null)
                        {
                            missileGate.TakeDamage(damageBullet);
                        }
                        break;
                }

                Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
