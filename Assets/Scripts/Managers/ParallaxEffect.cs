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
        // (EN TÉRMINOS DE DESPLAZAMIENTO)

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

        // LOS DESPLAZAMIENTOS EN LOS EJES DE X E Y SE EVALÚAN DE MANERA SEPARADA

        if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > spriteWidth / 1.25) {

            // Se identifica la posición inicial del sprite para después reasignarla
            Vector3 newSpritePosition = transform.position;


            // Desplazamiento a derecha o a izquierda, dependiendo de si la cámara se halla
            // más hacia la derecha o más hacia la izquierda que el sprite
            if (Camera.main.transform.position.x > transform.position.x)
            {
                newSpritePosition.x += spriteWidth;
            }
            else if (Camera.main.transform.position.x < transform.position.x)
            {
                newSpritePosition.x -= spriteWidth;
            }


            // Se reasigna la posición con los valores calculados, dependiendo del
            // desplazamiento a ejecutar
            transform.position = newSpritePosition;
        }


        if (Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > spriteHeight / 1.25) {
            // Se identifica la posición inicial del sprite para después reasignarla
            Vector3 newSpritePosition = transform.position;


            // Desplazamiento hacia arriba o hacia abajo, dependiendo de si la cámara se halla
            // más hacia arriba o más hacia abajo que el sprite
            if (Camera.main.transform.position.y > transform.position.y)
            {
                newSpritePosition.y += spriteHeight;
            }
            else if (Camera.main.transform.position.y < transform.position.y)
            {
                newSpritePosition.y -= spriteHeight;
            }


            // Se reasigna la posición con los valores calculados, dependiendo del
            // desplazamiento a ejecutar
            transform.position = newSpritePosition;
        }
    }
}
