using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPC : MonoBehaviour
{
    private int health = 3000;
    public GameObject explosionEffect;
    private SpriteRenderer spRd;
    private bool dead = false;


    // Start is called before the first frame update
    void Start()
    {
        spRd = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(ChangeColor());
        health -= damage;

        if (health <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(Die());
        }
    }


    private IEnumerator Die()
    {
        // Listado de las cinco posiciones en las que hay que hacer spawnear efectos de explosiones
        Vector3[] explosionPositions = {
            transform.position,
            transform.position + new Vector3(-0.75f, 0.5f, 0), // Arriba a la izquierda
            transform.position + new Vector3(0.75f, 0.5f, 0),  // Arriba a la derecha
            transform.position + new Vector3(-0.75f, -0.5f, 0), // Abajo a la izquierda
            transform.position + new Vector3(0.75f, -0.5f, 0)  // Abajo a la derecha
        };


        for (int j=0; j<2; j++)
        {
            for (int i = 0; i < explosionPositions.Length; i++)
            {
                Instantiate(explosionEffect, explosionPositions[i], Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }


    public IEnumerator ChangeColor()
    {
        spRd.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (health > 0)
        {
            spRd.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
