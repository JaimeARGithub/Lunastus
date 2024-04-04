using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargedBullet : MonoBehaviour
{
    [SerializeField] private float velocidad = 15f;
    private Rigidbody2D rb;
    private int damageCharged = 80;
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
            if (!collision.name.Equals("Bullet(Clone)") && !collision.name.Equals("ChargedBullet(Clone)") && !collision.name.Equals("Missile(Clone)")
                && !collision.name.Equals("SentinelBullet(Clone)"))
            {
                switch (collision.name)
                {
                    case string name when name.StartsWith("Crab"):
                        Crab crab = collision.GetComponent<Crab>();
                        if (crab != null)
                        {
                            crab.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("Octopus"):
                        Octopus octopus = collision.GetComponent<Octopus>();
                        if (octopus != null)
                        {
                            octopus.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("Jumper"):
                        Jumper jumper = collision.GetComponent<Jumper>();
                        if (jumper != null)
                        {
                            jumper.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("Fly"):
                        Fly fly = collision.GetComponent<Fly>();
                        if (fly != null)
                        {
                            fly.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("Eye"):
                        Eye eye = collision.GetComponent<Eye>();
                        if (eye != null)
                        {
                            eye.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("Rinofish"):
                        Rinofish rinofish = collision.GetComponent<Rinofish>();
                        if (rinofish != null)
                        {
                            rinofish.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("Tentacles"):
                        Tentacles tentacles = collision.GetComponent<Tentacles>();
                        if (tentacles != null)
                        {
                            tentacles.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("Shell"):
                        Shell shell = collision.GetComponent<Shell>();
                        if (shell != null)
                        {
                            shell.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("Biceraptor"):
                        Biceraptor biceraptor = collision.GetComponent<Biceraptor>();
                        if (biceraptor != null)
                        {
                            biceraptor.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("Sentinel"):
                        Sentinel sentinel = collision.GetComponent<Sentinel>();
                        if (sentinel != null)
                        {
                            sentinel.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("YellowRobot"):
                        YellowRobot yellowRobot = collision.GetComponent<YellowRobot>();
                        if (yellowRobot != null)
                        {
                            yellowRobot.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("GreyRobot"):
                        GreyRobot greyRobot = collision.GetComponent<GreyRobot>();
                        if (greyRobot != null)
                        {
                            greyRobot.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("BrownRobot"):
                        BrownRobot brownRobot = collision.GetComponent<BrownRobot>();
                        if (brownRobot != null)
                        {
                            brownRobot.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("RedRobot"):
                        RedRobot redRobot = collision.GetComponent<RedRobot>();
                        if (redRobot != null)
                        {
                            redRobot.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("Gate"):
                        Gate gate = collision.GetComponent<Gate>();
                        if (gate != null)
                        {
                            gate.TakeDamage(damageCharged);
                        }
                        break;
                    case string name when name.StartsWith("MissileGate"):
                        MissileGate missileGate = collision.GetComponent<MissileGate>();
                        if (missileGate != null)
                        {
                            missileGate.TakeDamage(damageCharged);
                        }
                        break;
                }

                Vector3 impactPosition = transform.position + new Vector3(0.5f, 0f, 0f);
                Instantiate(impactEffect, impactPosition, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
