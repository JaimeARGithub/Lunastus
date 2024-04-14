using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowRobotMovementShooting : MonoBehaviour
{
    private GameObject hunter;
    private float distance;

    private bool mirandoDerecha = true;

    private float chaseSpeed = 2f;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        transform.Rotate(0f, 180f, 0f);
    }
}
