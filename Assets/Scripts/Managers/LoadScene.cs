using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // La clase LoadScene, asignada a la parte posterior de las compuertas, lo único
    // que hace es recibir el NOMBRE de la escena que debe cargar y buscar en la
    // jerarquía un objeto LevelLoader, emplazado en un canvas para poder hacer el
    // crossfade.

    // Cuando se detecta la colisión con el jugador, se llama al método del LevelLoader
    // que se encarga de cargar escena y de generar el crossfade con un cierto retardo.

    // Se podría meter aquí el Scene Manager con la carga de escena y de hecho solía
    // estar aquí, pero por consistencia entre llamada al Scene Manager y tiempos de espera
    // establecidos en la corrutina se ha preferido concentrarlo solo en una sola clase.

    public string SceneName;
    private LevelLoader l;

    private void Start()
    {
        l = FindObjectOfType<LevelLoader>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Hunter"))
        {
            StartCoroutine(l.LoadScene(SceneName));
        }
    }
}
