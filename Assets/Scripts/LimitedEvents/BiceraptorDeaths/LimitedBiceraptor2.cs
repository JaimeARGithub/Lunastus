using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedBiceraptor2 : MonoBehaviour
{
    private GameManager gameManager;
    private Biceraptor b;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        b = GetComponent<Biceraptor>();

        if (gameManager.GetBraptor2Killed())
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (b.isDead())
        {
            gameManager.SetBraptor2Killed();
        }
    }
}
