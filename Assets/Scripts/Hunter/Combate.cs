using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combate : MonoBehaviour
{
    // Para la ejecución de animaciones
    public Animator animator;

    // Para detectar si se está saltando
    public bool isGrounded;
    public LayerMask groundLayer;
    public Collider2D piesCollider;

    // Para detectar si se está en movimiento
    private float movimientoH;

    // Para los cooldowns
    private bool misilDesbloqueado = true; // VALOR EXTRAÍDO DEL GAME MANAGER, CAMBIARLO DESPUÉS
    private float tiempoTranscurridoDisparo = 0f;
    private float tiempoEsperaDisparo = 0.2f;
    private float tiempoTranscurridoMisil = 0f;
    private float tiempoEsperaMisil = 2f;

    // Para los sonidos
    public AudioSource sonidoDisparo;
    public AudioSource sonidoMisil;

    // Para los disparos
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;



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
            sonidoDisparo.Play();
            AnimarDisparo();
            Disparar();
            tiempoTranscurridoDisparo = 0f;
        }
        tiempoTranscurridoDisparo += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.R) && tiempoTranscurridoMisil >= tiempoEsperaMisil && misilDesbloqueado)
        {
            sonidoMisil.Play();
            AnimarDisparo();
            DispararMisil();
            tiempoTranscurridoMisil = 0f;
        }
        tiempoTranscurridoMisil += Time.deltaTime;
    }


    private void Disparar()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }


    private void DispararMisil()
    {
        Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
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
        animator.SetBool("isShootingIdle", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isShootingIdle", false);
    }

    private IEnumerator ShootRunning()
    {
        animator.SetBool("isShootingRunning", true);
        animator.SetBool("isRunning", false);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isShootingRunning", false);
        animator.SetBool("isRunning", true);
    }

    private IEnumerator ShootJumping()
    {
        animator.SetBool("isShootingJumping", true);
        animator.SetBool("isJumping", false);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isShootingJumping", false);
        animator.SetBool("isJumping", true);
    }
}
