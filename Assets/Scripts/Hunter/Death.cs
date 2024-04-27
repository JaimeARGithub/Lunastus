using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
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
