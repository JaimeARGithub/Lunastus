using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combate : MonoBehaviour
{
    // Para la ejecución de animaciones
    public Animator animator;

    // Para detectar si se está saltando
    private bool isGrounded = false;
    public LayerMask groundLayer;
    public Collider2D piesCollider;

    // Para detectar si se está en movimiento
    private float movimientoH;

    // Para los cooldowns
    private float tiempoTranscurridoDisparo = 0f;
    private float tiempoEsperaDisparo = 0.2f;
    private float tiempoTranscurridoMisil = 0f;
    private float tiempoEsperaMisil = 0.2f;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && tiempoTranscurridoDisparo >= tiempoEsperaDisparo)
        {
            Shoot();
            tiempoTranscurridoDisparo = 0f;
        }
        tiempoTranscurridoDisparo += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.R) && tiempoTranscurridoMisil >= tiempoEsperaMisil)
        {
            ShootMissile();
            tiempoTranscurridoMisil = 0f;
        }
        tiempoTranscurridoMisil += Time.deltaTime;
    }


    private void Shoot()
    {
        AnimarDisparo();
    }


    private void ShootMissile()
    {
        AnimarDisparo();
    }

    private void AnimarDisparo()
    {
        movimientoH = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.IsTouchingLayers(piesCollider, groundLayer);


        if (isGrounded)
        {
            if (movimientoH == 0)
            {
                StartCoroutine(ShootIdle());
            }
            else
            {
                StartCoroutine(ShootRunning());
            }
        }
        else
        {
            StartCoroutine(ShootJumping());
        }
    }


    private IEnumerator ShootIdle()
    {
        animator.SetBool("isShooting", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isShooting", false);
    }

    private IEnumerator ShootRunning()
    {
        animator.SetBool("isShooting", true);
        animator.SetBool("isRunning", false);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isShooting", false);
        animator.SetBool("isRunning", true);
    }

    private IEnumerator ShootJumping()
    {
        animator.SetBool("isShooting", true);
        animator.SetBool("isJumping", false);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isShooting", false);
        animator.SetBool("isJumping", true);
    }
}
