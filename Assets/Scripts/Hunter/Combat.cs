using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Combat : MonoBehaviour
{
    // Para la ejecución de animaciones
    private Animator animator;

    // Para detectar si se está saltando
    public bool isGrounded;
    public LayerMask groundLayer;
    public Collider2D piesCollider;

    // Para detectar si se está en movimiento
    private float movimientoH;

    // Para los cooldowns
    private bool misilDesbloqueado = false;      // VALOR QUE DEPENDE DEL GAME MANAGER
                                                // CAMBIARLO PARA QUE LEA EL VALOR DE ÉL EN EL UPDATE

    private int misilesRestantes = 0;           // VALOR QUE DEPENDE DEL GAME MANAGER
                                                // CAMBIARLO PARA QUE LEA EL VALOR DE ÉL EN EL UPDATE

    private float tiempoTranscurridoDisparo = 0f;
    private float tiempoEsperaDisparo = 0.2f;
    private float tiempoTranscurridoMisil = 0f;
    private float tiempoEsperaMisil = 2f;

    // Para los sonidos
    public AudioSource sonidoDisparo;
    public AudioSource sonidoMisil;
    public AudioSource sonidoDisparoCargado;
    public AudioSource sonidoCargando;

    // Para los disparos
    public Transform firePointStanding;
    public Transform firePointCrouch;
    public Transform firePointUp;
    private Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public GameObject chargedPrefab;
    public GameObject chargeReadyEffect;
    private GameObject currentChargeEffect;

    // Variables de tiempo para el disparo cargado
    private float instantePresionBoton = 0f;
    private float tiempoRequeridoInicioCarga = 0.3f;
    private float tiempoRequeridoPresionDisparoCargado = 2f;
    private float tiempoTranscurridoSonidoCarga = 0f;
    private float tiempoEsperaSonidoCarga = 0.7f;
    private bool disparoCargadoDisponible = false;

    // Variables para el parpadeo de color durante la carga y el efecto de
    // brillo cuando la carga se completa
    private SpriteRenderer spRd;
    private bool colorCambiado = false;
    private float intervaloCambioColor = 0.1f;
    private float instanteUltimoCambioColor = 0f;

    // Para manipular el collider del jugador dependiendo de si se agacha o no
    public Collider2D standingCollider;
    public Collider2D crouchCollider;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spRd = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // LEER DEL GAME MANAGER SI LOS MISILES ESTÁN DESBLOQUEADOS
        // LEER DEL GAME MANAGER LA MUNICIÓN RESTANTE DE MISILES

        isGrounded = Physics2D.IsTouchingLayers(piesCollider, groundLayer);
        if (isGrounded && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow)))
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                standingCollider.enabled = false;
                crouchCollider.enabled = true;
                firePoint = firePointCrouch;
            } else if (Input.GetKey(KeyCode.UpArrow))
            {
                standingCollider.enabled = true;
                crouchCollider.enabled = false;
                firePoint = firePointUp;
            } 
            
        }
        else
        {
            standingCollider.enabled = true;
            crouchCollider.enabled = false;
            firePoint = firePointStanding;
        }



        if (Input.GetKeyDown(KeyCode.E) && tiempoTranscurridoDisparo >= tiempoEsperaDisparo)
        {
            sonidoDisparo.Play();
            AnimarDisparo();
            Disparar();
            tiempoTranscurridoDisparo = 0f;

            // Para el inicio de la carga
            instantePresionBoton = Time.time;
        }
        tiempoTranscurridoDisparo += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.R) && tiempoTranscurridoMisil >= tiempoEsperaMisil && misilDesbloqueado && misilesRestantes > 0)
        {
            sonidoMisil.Play();
            AnimarDisparo();
            DispararMisil();
            tiempoTranscurridoMisil = 0f;
        }
        tiempoTranscurridoMisil += Time.deltaTime;

        
        if (Input.GetKey(KeyCode.E))
        {
            // Código a ejecutar mientras se mantenga presionada la tecla E

            if (Time.time - instantePresionBoton >= tiempoRequeridoInicioCarga)
            {
                // Si se ha mantenido presionada durante el tiempo requerido para comenzar la carga (0.1f):
                // empieza a reproducirse el sonido de carga y a parpadear en azul el sprite
                if (Time.time - instanteUltimoCambioColor >= intervaloCambioColor)
                {
                    ChangeColor();
                    instanteUltimoCambioColor = Time.time;
                }


                if (!sonidoCargando.isPlaying && tiempoTranscurridoSonidoCarga >= tiempoEsperaSonidoCarga)
                {
                    sonidoCargando.Play();
                    tiempoTranscurridoSonidoCarga = 0f;
                }
                tiempoTranscurridoSonidoCarga += Time.deltaTime;


                if (Time.time - instantePresionBoton >= tiempoRequeridoPresionDisparoCargado)
                {
                    // Disparo cargado disponible: ajustamos booleana de la que dependen el disparo y el objeto hijo que brilla
                    disparoCargadoDisponible = true;
                }
            }
        }


        if (disparoCargadoDisponible && currentChargeEffect == null)
        {
            currentChargeEffect = Instantiate(chargeReadyEffect, firePoint.position, firePoint.rotation);
            currentChargeEffect.transform.parent = transform;
        }
        if (disparoCargadoDisponible && currentChargeEffect != null)
        {
            currentChargeEffect.transform.position = firePoint.position;
        }


        if (Input.GetKeyUp(KeyCode.E))
        {
            sonidoCargando.Stop();

            spRd.color = Color.white;
            colorCambiado = false;

            if (disparoCargadoDisponible)
            {
                if (currentChargeEffect != null)
                {
                    Destroy(currentChargeEffect);
                }

                sonidoDisparoCargado.Play();
                AnimarDisparo();
                DispararCargado();

                // Restablecer el tiempo de presión para poder ejecutar un nuevo disparo cargado
                instantePresionBoton = 0f;
                disparoCargadoDisponible = false;
            }
        }
    }

    public void activarMisiles()
    {
        // USAR LOS SETTERS DEL GAME MANAGER
        misilDesbloqueado = true;
        misilesRestantes += 5;
    }

    public void recargarMisiles()
    {
        if (misilDesbloqueado)
        {
            misilesRestantes += 5;
        }
    }

    private void ChangeColor()
    {
        if (!colorCambiado)
        {
            spRd.color = Color.cyan;
        } else
        {
            spRd.color = Color.white;
        }

        colorCambiado = !colorCambiado;
    }


    private void Disparar()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }


    private void DispararMisil()
    {
        Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
        misilesRestantes -= 1;      // CAMBIAR ESTA LÍNEA PARA QUE REDUZCA LA
                                    // CANTIDAD DE MISILES RESTANTES DEL GAME MANAGER
    }

    private void DispararCargado()
    {
        Instantiate(chargedPrefab, firePoint.position, firePoint.rotation);
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


    private IEnumerator ShootJumping()
    {
        animator.SetBool("isShootingJumping", true);
        animator.SetBool("isJumping", false);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isShootingJumping", false);
        animator.SetBool("isJumping", true);
    }
}
