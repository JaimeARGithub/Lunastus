using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedMissileAugment4 : MonoBehaviour
{
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.GetAugmMissiles4())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Hunter"))
        {
            Combat c = collision.GetComponent<Combat>();
            if (c.getMisilesDesbloqueados())
            {
                gameManager.SetAugmMissiles4();
            }
        }
    }
}
