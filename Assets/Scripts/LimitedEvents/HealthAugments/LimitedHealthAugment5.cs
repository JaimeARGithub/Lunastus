using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedHealthAugment5 : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.GetAugmHealth5())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Hunter") && !gameManager.GetAugmHealth5())
        {
            gameManager.SetAugmHealth5();
        }
    }
}
