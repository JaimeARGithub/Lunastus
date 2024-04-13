using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusMovement : MonoBehaviour
{
    [SerializeField] private bool mirandoDerecha = false;
    private GameObject hunter;
    private Rigidbody2D rb;
    private Octopus o;
    private float speed = 2.5f;
    private float distance;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        o = GetComponent<Octopus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!o.isDead())
        {
            distance = Vector2.Distance(transform.position, hunter.transform.position);

            if (distance < 4)
            {

                if (hunter.transform.position.x <= transform.position.x && !mirandoDerecha)
                {
                    Girar();
                } else if (hunter.transform.position.x > transform.position.x && mirandoDerecha)
                {
                    Girar();
                }


                if (hunter.transform.position.x <= transform.position.x)
                {
                    rb.velocity = new Vector2(1*speed, rb.velocity.y);
                } else if (hunter.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-1*speed, rb.velocity.y);
                }



            } else
            {

                rb.velocity = new Vector2(0, rb.velocity.y);
                if (mirandoDerecha)
                {
                    Girar();
                }

            }
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        transform.Rotate(0f, 180f, 0f);
    }
}
