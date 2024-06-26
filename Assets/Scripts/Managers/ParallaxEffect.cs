using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // Multiplicador de velocidad
    public float parallaxMultiplier = 0.5f;

    // Posici�n de la c�mara en el sprite anterior
    private Vector3 previousCameraPosition;

    // Dimensiones del sprite
    private Vector2 spriteSize;
    private float spriteWidth;
    private float spriteHeight;

    private void Start()
    {
        // En el start ubicamos las dimensiones del sprite y a la posici�n anterior de la c�mara
        // le asignamos la actual
        previousCameraPosition = Camera.main.transform.position;
        spriteSize = GetComponent<SpriteRenderer>().bounds.size;
        spriteWidth = spriteSize.x;
        spriteHeight = spriteSize.y;
    }

    private void LateUpdate()
    {
        // TRAS CADA FRAME, calculamos mediante un Vector3 la distancia entre la
        // posici�n actual de la c�mara y la posici�n anterior de la c�mara
        // (EN T�RMINOS DE DESPLAZAMIENTO)

        // Al sprite del parallax le aplicamos una variaci�n del transform
        // en los ejes X e Y igual a la distancia entre las posiciones actual
        // y anterior de la c�mara por un coeficiente de paralaje

        // A la posici�n anterior de la c�mara le asignamos la posici�n actual

        Vector3 delta = Camera.main.transform.position - previousCameraPosition;
        transform.position += new Vector3(delta.x * parallaxMultiplier, delta.y * parallaxMultiplier, 0f);
        previousCameraPosition = Camera.main.transform.position;



        // Para dar sensaci�n de continuidad al sprite del paralaje, se eval�a que :
        // --El valor absoluto de la resta entre la X de la posici�n del sprite y la X de la posici�n de la c�mara supere a la mitad del ancho
        // --El valor absoluto de la resta entre la Y de la posici�n del sprite y la Y de la posici�n de la c�mara supere a la mitad del alto

        // LOS DESPLAZAMIENTOS EN LOS EJES DE X E Y SE EVAL�AN DE MANERA SEPARADA

        if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > spriteWidth / 1.25) {

            // Se identifica la posici�n inicial del sprite para despu�s reasignarla
            Vector3 newSpritePosition = transform.position;


            // Desplazamiento a derecha o a izquierda, dependiendo de si la c�mara se halla
            // m�s hacia la derecha o m�s hacia la izquierda que el sprite
            if (Camera.main.transform.position.x > transform.position.x)
            {
                newSpritePosition.x += spriteWidth;
            }
            else if (Camera.main.transform.position.x < transform.position.x)
            {
                newSpritePosition.x -= spriteWidth;
            }


            // Se reasigna la posici�n con los valores calculados, dependiendo del
            // desplazamiento a ejecutar
            transform.position = newSpritePosition;
        }


        if (Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > spriteHeight / 1.25) {
            // Se identifica la posici�n inicial del sprite para despu�s reasignarla
            Vector3 newSpritePosition = transform.position;


            // Desplazamiento hacia arriba o hacia abajo, dependiendo de si la c�mara se halla
            // m�s hacia arriba o m�s hacia abajo que el sprite
            if (Camera.main.transform.position.y > transform.position.y)
            {
                newSpritePosition.y += spriteHeight;
            }
            else if (Camera.main.transform.position.y < transform.position.y)
            {
                newSpritePosition.y -= spriteHeight;
            }


            // Se reasigna la posici�n con los valores calculados, dependiendo del
            // desplazamiento a ejecutar
            transform.position = newSpritePosition;
        }
    }
}
