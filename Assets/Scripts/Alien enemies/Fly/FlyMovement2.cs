using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement2 : MonoBehaviour
{
    private GameObject hunter;
    private Rigidbody2D rb;
    private Fly f;

    // Variables para la persecuci�n
    private float distance;
    private float chaseSpeed = 2f;
    [SerializeField] private bool mirandoDerecha = false;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        f = GetComponent<Fly>();
    }

    // Update is called once per frame
    void Update()
    {
        // L�gica de mosca no patrullera: permanece est�tica hasta que el jugador entra
        // en un radio de detecci�n.
        // Mientras el jugador permanece en el radio de detecci�n, es perseguido. Si sale,
        // la mosca vuelve a quedarse est�tica.
        if (!f.isDead())
        {
            distance = Vector2.Distance(transform.position, hunter.transform.position);

            if (distance > 6)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            } else
            {
                Vector2 direction = (hunter.transform.position - transform.position);
                rb.velocity = direction * chaseSpeed;


                if (hunter.transform.position.x <= transform.position.x && mirandoDerecha)
                {
                    Girar();
                }
                else if (hunter.transform.position.x > transform.position.x && !mirandoDerecha)
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
