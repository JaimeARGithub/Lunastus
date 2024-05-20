using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    // Lo óptimo sería manejar el daño de lava según continuidad del contacto de colliders (OnTriggerStay2D),
    // pero Unity no maneja ésto cuando los elementos se encuentran en reposo, lo cual da lugar a que el
    // jugador pueda estar dentro de la lava quieto y no recibir daño

    // Por ello, se va a hacer mediante una corrutina que, en bucle infinito, aplica daño
    // Cuando el jugador entra en la lava, se inicia, y cuando sale de ella, se detiene
    public AudioSource lavaSound;
    private int lavaDamage = 10;
    private float damageInterval = 1f;


    // Es necesario almacenar ESPECÍFICAMENTE la instancia de la corrutina que estamos empleando para hacer el
    // daño de manera continuada, puesto que en caso contrario, la parada de la corrutina no sabe qué corrutina
    // tiene que parar, y el jugador continúa recibiendo daño aun tras salir de la lava
    // Es necesario almacenar una referencia a la corrutina
    private Coroutine lavaDamageCoroutine;

    // También es necesario igualar la referencia a la corrutina a null cuando el jugador sale de la lava, puesto
    // que para que la corrutina comience a ejecutarse en el OnTriggerEnter es requerimiento que tenga valor nulo,
    // y en el OnTriggerExit solamente la estamos deteniendo, con lo que la corrutina está parada, pero no vale
    // nulo, y luego al volver a entrar en la lava no se ejecuta



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health h = collision.gameObject.GetComponent<Health>();
            // Si health no es null y NO HAY CORRUTINA DE DAÑO DE LAVA INICIADA:
            // EMPEZARLA
            if (h != null  &&  lavaDamageCoroutine == null)
            {
                lavaDamageCoroutine = StartCoroutine(ApplyLavaDamage(h));
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health h = collision.gameObject.GetComponent<Health>();
            // Si health no es null y SI HAY CORRUTINA DE DAÑO DE LAVA INICIADA:
            // DETENERLA
            // E IGUALARLA A NULL, PARA QUE AL VOLVER A ENTRAR EN LA LAVA SE VUELVA A ACTIVAR
            if (h != null  &&  lavaDamageCoroutine != null)
            {
                StopCoroutine(lavaDamageCoroutine);
                lavaDamageCoroutine = null;
            }
        }
    }


    private IEnumerator ApplyLavaDamage(Health health)
    {
        while (true)
        {
            if (health.isVulnerable())
            {
                lavaSound.Play();
                health.TakeDamage(lavaDamage);
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
