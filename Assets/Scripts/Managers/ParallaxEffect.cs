using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // Multiplicador de velocidad
    public float parallaxMultiplier = 0.5f;

    // Posición de la cámara en el sprite anterior
    private Vector3 previousCameraPosition;

    // Dimensiones del sprite
    private Vector2 spriteSize;
    private float spriteWidth;
    private float spriteHeight;

    private void Start()
    {
        // En el start ubicamos las dimensiones del sprite y a la posición anterior de la cámara
        // le asignamos la actual
        previousCameraPosition = Camera.main.transform.position;
        spriteSize = GetComponent<SpriteRenderer>().bounds.size;
        spriteWidth = spriteSize.x;
        spriteHeight = spriteSize.y;
    }

    private void LateUpdate()
    {
        // TRAS CADA FRAME, calculamos mediante un Vector3 la distancia entre la
        // posición actual de la cámara y la posición anterior de la cámara

        // Al sprite del parallax le aplicamos una variación del transform
        // en los ejes X e Y igual a la distancia entre las posiciones actual
        // y anterior de la cámara por un coeficiente de paralaje

        // A la posición anterior de la cámara le asignamos la posición actual

        Vector3 delta = Camera.main.transform.position - previousCameraPosition;
        transform.position += new Vector3(delta.x * parallaxMultiplier, delta.y * parallaxMultiplier, 0f);
        previousCameraPosition = Camera.main.transform.position;



        // Para dar sensación de continuidad al sprite del paralaje, se evalúa que :
        // --El valor absoluto de la resta entre la X de la posición del sprite y la X de la posición de la cámara supere a la mitad del ancho
        // --El valor absoluto de la resta entre la Y de la posición del sprite y la Y de la posición de la cámara supere a la mitad del alto

        if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > spriteWidth / 2 ||
            Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > spriteHeight / 2)
        {
            // Reposition sprite to the opposite side
            Vector3 newSpritePosition = transform.position;

            if (Camera.main.transform.position.x > transform.position.x)
            {
                newSpritePosition.x += spriteWidth;
            }
            else if (Camera.main.transform.position.x < transform.position.x)
            {
                newSpritePosition.x -= spriteWidth;
            }


            if (Camera.main.transform.position.y > transform.position.y)
            {
                newSpritePosition.y += spriteHeight;
            }
            else if (Camera.main.transform.position.y < transform.position.y)
            {
                newSpritePosition.y -= spriteHeight;
            }


            transform.position = newSpritePosition;
        }
    }
}
