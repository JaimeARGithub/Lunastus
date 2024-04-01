using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAmmo : MonoBehaviour
{
    public AudioSource sound;
    private SpriteRenderer spRd;
    public GameObject effect;
    public AudioSource errorSound;
    public GameObject errorEffect;


    // Start is called before the first frame update
    void Start()
    {
        spRd = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Hunter"))
        {
            Combat c = collision.GetComponent<Combat>();

            if (c.getMisilesDesbloqueados())
            {
                Color colorSprite = spRd.material.color;
                colorSprite.a = 0f;
                spRd.material.color = colorSprite;
                Destroy(GetComponent<Collider2D>());

                Instantiate(effect, transform.position, Quaternion.identity);
                sound.Play();
                c.recargarMisiles();

                Destroy(gameObject, 2f);
            } else
            {
                Instantiate(errorEffect, transform.position, Quaternion.identity);
                errorSound.Play();
            }
        }
    }
}
