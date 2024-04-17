using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedHealthAugment2 : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.GetAugmHealth2())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Hunter"))
        {
            gameManager.SetAugmHealth2();
        }
    }
}
