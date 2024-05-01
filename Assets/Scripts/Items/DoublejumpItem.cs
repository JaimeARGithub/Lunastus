using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublejumpItem : MonoBehaviour
{
    private GameManager gameManager;

    public AudioSource sound;
    private SpriteRenderer spRd;
    public GameObject effect;


    // Start is called before the first frame update
    void Start()
    {
        spRd = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.GetDoublejumpUnlocked())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Hunter") && !gameManager.GetDoublejumpUnlocked())
        {
            Color colorSprite = spRd.material.color;
            colorSprite.a = 0f;
            spRd.material.color = colorSprite;
            Destroy(GetComponent<Collider2D>());

            Instantiate(effect, transform.position, Quaternion.identity);
            sound.Play();
            Movement m = collision.GetComponent<Movement>();
            m.activarDobleSalto();

            Destroy(gameObject, 2f);
        }
    }
}
