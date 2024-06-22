using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMovement : MonoBehaviour
{
    // Variables para límites de movimiento en el eje Y
    private float maxY = -10.5f;
    private float minY = -21f;

    // Variables para control del movimiento vertical
    private bool movingUpwards = true;
    private float speed = 0.25f;


    // Update is called once per frame
    void Update()
    {
        if (movingUpwards)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, maxY), speed * Time.deltaTime);
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, minY), speed * Time.deltaTime);
        }


        if (transform.position.y == maxY)
        {
            movingUpwards = false;
        }
        if (transform.position.y == minY)
        {
            movingUpwards = true;
        }
    }
}
