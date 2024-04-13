using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelMovement : MonoBehaviour
{
    private GameObject hunter;
    [SerializeField] private bool mirandoDerecha = true;
    private float distance;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, hunter.transform.position);

        if (distance <= 10)
        {
            if ((mirandoDerecha && hunter.transform.position.x < transform.position.x) || 
                (!mirandoDerecha && hunter.transform.position.x > transform.position.x))
            {
                Girar();
            }
        }
    }


    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        transform.Rotate(0f, 180f, 0f);
    }
}
