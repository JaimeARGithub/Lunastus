using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileItem : MonoBehaviour
{
    public AudioSource sound;
    private SpriteRenderer spRd;
    public GameObject effect;


    // Start is called before the first frame update
    void Start()
    {
        spRd = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Hunter"))
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
