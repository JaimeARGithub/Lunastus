using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Combat : MonoBehaviour
{
    private GameManager gameManager;
    private Movement movement;

    // PARA MOSTRAR COSAS EN IU
    public UnityEngine.UI.Image ammoImage;
    public TextMeshProUGUI ammoText;

    // Para la ejecución de animaciones
    private Animator animator;

    // Para detectar si se está saltando
    public bool grounded;
    public LayerMask groundLayer;
    // Para el box cast
    private float boxOffsetX = 0.125f;
    private Vector2 boxSize = new Vector2(0.5f, 0.25f);
    private float castDistance = 0.83f;


    // Para detectar si se está en movimiento
    private float movimientoH;

    // Para los cooldowns
    //private bool misilDesbloqueado = false;      // VALOR QUE DEPENDE DEL GAME MANAGER
                                                 // CAMBIARLO PARA QUE LEA EL VALOR DE ÉL EN EL UPDATE

    //private int limiteMisiles = 0;              // VALOR QUE DEPENDE DEL GAME MANAGER
                                                // CAMBIARLO PARA QUE LEA EL VALOR DE ÉL EN EL UPDATE

    //private int municionMisiles = 0;           // VALOR QUE DEPENDE DEL GAME MANAGER
                                               // CAMBIARLO PARA QUE LEA EL VALOR DE ÉL EN EL UPDATE

    private float tiempoTranscurridoDisparo = 0f;
    private float tiempoEsperaDisparo = 0.15f;
    private float tiempoTranscurridoMisil = 0f;
    private float tiempoEsperaMisil = 1f;

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
        gameManager = FindObjectOfType<GameManager>();
        movement = GetComponent<Movement>();

        animator = GetComponent<Animator>();
        spRd = GetComponent<SpriteRenderer>();


        // De inicio, la imagen y el texto de la munición son invisibles
        // En el update se lee si los misiles se han desbloqueado y entonces se muestran
        ammoImage.enabled = false;
        Color textColor = ammoText.color;
        textColor.a = 0f;
        ammoText.color = textColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gamePaused)
        {
            // LEER DEL GAME MANAGER SI LOS MISILES ESTÁN DESBLOQUEADOS
            // LEER DEL GAME MANAGER EL LÍMITE DE MISILES
            // LEER DEL GAME MANAGER LA MUNICIÓN RESTANTE DE MISILES
            if (gameManager.GetMissileUnlocked())
            {
                ammoImage.enabled = true;
                Color textColor = ammoText.color;
                textColor.a = 1f;
                ammoText.color = textColor;

                ammoText.text = "x " + gameManager.GetCurrentMissiles().ToString("00") + "/" + gameManager.GetMaxMissiles().ToString("00");
            }



            if (isGrounded() && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow)))
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    standingCollider.enabled = false;
                    crouchCollider.enabled = true;
                    firePoint = firePointCrouch;
                }
                else if (Input.GetKey(KeyCode.UpArrow))
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


            if (Input.GetKeyDown(KeyCode.R) && tiempoTranscurridoMisil >= tiempoEsperaMisil && gameManager.GetMissileUnlocked() && gameManager.GetCurrentMissiles() > 0)
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
    }

    public bool getMisilesDesbloqueados()
    {
        return gameManager.GetMissileUnlocked();
    }

    public void activarMisiles()
    {
        gameManager.SetMissileUnlocked();
    }

    public void ampliarMisiles()
    {
        gameManager.AugmentMissiles();
    }

    public void recargarMisiles()
    {
        gameManager.RechargeMissiles();
    }

    private void ChangeColor()
    {
        if (!colorCambiado)
        {
            spRd.color = Color.yellow;
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
        gameManager.ExpendMissile();         // CAMBIAR ESTA LÍNEA PARA QUE REDUZCA LA
                                            // CANTIDAD DE MISILES RESTANTES DEL GAME MANAGER
    }

    private void DispararCargado()
    {
        Instantiate(chargedPrefab, firePoint.position, firePoint.rotation);
    }

    private void AnimarDisparo()
    {
        movimientoH = Input.GetAxisRaw("Horizontal");

        if (isGrounded())
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


    private bool isGrounded()
    {
        // Castear una caja en mi posición, del tamaño indicado, giro de 0 grados,
        // hacia abajo, a la distancia indicada y contra la layer del suelo
        // Para que coincida exactamente con la hitbox del jugador en verticalidad, se le
        // suma o resta un offset dependiendo de a dónde esté mirando el jugador
        if (movement.GetMirandoDerecha() && Physics2D.BoxCast((transform.position + new Vector3(boxOffsetX, 0f, 0f)), boxSize, 0, -transform.up, castDistance, groundLayer) ||
            !movement.GetMirandoDerecha() && Physics2D.BoxCast((transform.position - new Vector3(boxOffsetX, 0f, 0f)), boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        return grounded;
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
