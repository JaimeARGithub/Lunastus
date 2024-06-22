using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    // Cuando el Hunter muere, no se puede destruir el gameObject al instante; es necesario dejar un
    // pequeño delay para que la animación y el sonido se reproduzcan correctamente
    // Dado que hay que hacer muchos ajustes en el gameObject del Hunter para conseguir correctamente
    // esta "muerte tardía" Y se da lugar a inconsistencias en el comportamiento de los enemigos que
    // trackean al Hunter, se ha optado por destruir su gameObject al instante y meter la lógica de
    // la muerte (sonido, cambio de escena) en el efecto de la muerte
    public AudioSource deathSound;
    private GameManager gameManager;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(Die());
    }

    
    private IEnumerator Die()
    {
        // 1.- Reproducir sonido de muerte y destrucción de la parte visible en cuanto termina la animación
        deathSound.Play();
        Destroy(GetComponent<SpriteRenderer>(), 0.417f);


        // 2.- Espera de 3 segundos para destruir el objeto, para que el audio se escuche bien
        yield return new WaitForSeconds(3f);


        // 3.- Game over definitivo; setter y destrucción del objeto
        gameManager.GameOverState();
        Debug.Log("GAME OVER");
        Destroy(gameObject);
    }
}
