using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedMissileAugment2 : MonoBehaviour
{
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.GetAugmMissiles2())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Hunter") && !gameManager.GetAugmMissiles2())
        {
            Combat c = collision.GetComponent<Combat>();
            if (c.getMisilesDesbloqueados())
            {
                gameManager.SetAugmMissiles2();
            }
        }
    }
}
