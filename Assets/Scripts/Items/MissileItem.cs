using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileItem : MonoBehaviour
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

        if (gameManager.GetMissileUnlocked())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Hunter") && !gameManager.GetMissileUnlocked())
        {
            Color colorSprite = spRd.material.color;
            colorSprite.a = 0f;
            spRd.material.color = colorSprite;
            Destroy(GetComponent<Collider2D>());

            Instantiate(effect, transform.position, Quaternion.identity);
            sound.Play();
            Combat c = collision.GetComponent<Combat>();
            c.activarMisiles();

            Destroy(gameObject, 2f);
        }
    }
}
