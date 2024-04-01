using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public AudioSource openSound;
    public AudioSource closeSound;
    private int health = 20;
    private Animator animator;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Open();
        }
    }

    private void Open()
    {
        rb.gravityScale = 0f;
        animator.SetBool("isOpen", true);
        openSound.Play();
        Destroy(GetComponent<Collider2D>());
    }
}
