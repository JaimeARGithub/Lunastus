using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private SpriteRenderer spRd;
    public AudioSource saveSound;

    // Start is called before the first frame update
    void Start()
    {
        spRd = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si lo que triggerea el SavePoint es el cazador:
        if (collision.name.Equals("Hunter"))
        {
            // En primer lugar, se ejecuta el cambio de color a parpadeo en verde y se reproduce el sonido
            StartCoroutine(ChangeColor());
            saveSound.Play();

            // En segundo lugar, se ubica el componente Health del cazador para iniciar la cura progresiva
            Health h = collision.GetComponent<Health>();
            StartCoroutine(h.fullRestoration());

            // Finalmente, se guardan los datos de la partida
        }
    }

    private IEnumerator ChangeColor()
    {
        for (int i=0; i<5; i++)
        {
            spRd.color = Color.green;
            yield return new WaitForSeconds(0.25f);
            spRd.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
