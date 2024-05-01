using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedHealthAugment3 : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.GetAugmHealth3())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Hunter") && !gameManager.GetAugmHealth3())
        {
            gameManager.SetAugmHealth3();
        }
    }
}
