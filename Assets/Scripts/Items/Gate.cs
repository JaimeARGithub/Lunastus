using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private float originalGravity = 0f;
    private Collider2D gatecollider;

    public AudioSource openSound;
    public AudioSource closeSound;

    private int maxHealth = 20;
    private int health = 20;
    private bool isOpen = false;
    private float openingMoment;
    


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        gatecollider = GetComponent<Collider2D>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Open();
        }
    }


    private void Update()
    {
        if (isOpen && Time.time - openingMoment >= 3f)
        {
            StartCoroutine(Close());
        }
    }


    private void Open()
    {
        rb.gravityScale = 0f;
        gatecollider.enabled = false;

        isOpen = true;
        animator.SetBool("isOpen", true);
        animator.SetBool("isIdle", false);
        openSound.Play();

        openingMoment = Time.time;
    }


    private IEnumerator Close()
    {
        animator.SetBool("isClosed", true);
        animator.SetBool("isOpen", false);
        closeSound.Play();

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("isIdle", true);
        animator.SetBool("isClosed", false);


        isOpen = false;
        gatecollider.enabled = true;

        rb.gravityScale = originalGravity;
        health = maxHealth;
    }
}
