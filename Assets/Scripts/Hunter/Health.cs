using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    private GameManager gameManager;

    // PARA MOSTRAR COSAS EN IU
    public TextMeshProUGUI healthText;

    //private int maxHealth;
    //private int currentHealth;
    private HealthBar healthBar;
    public AudioSource damageSound;
    public GameObject deathAnimation;

    private SpriteRenderer spRd;
    private bool vulnerable = true;
    private float invulnerableInstant = 0f;
    private float invulnerabilityTime = 1f;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        //maxHealth = gameManager.GetMaxHealth(); // CAMBIARLO PARA QUE EN EL START LEA DEL GAME MANAGER
        //currentHealth = gameManager.GetCurrentHealth(); // CAMBIARLO PARA QUE EN EL START LEA DEL GAME MANAGER
        healthBar = FindObjectOfType<HealthBar>();
        healthBar.SetMaxHealth(gameManager.GetMaxHealth());

        spRd = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // LEER TODO EL RATO VIDA ACTUAL Y VIDA M�XIMA CON GETTERS DEL GAME MANAGER
        // Y AJUSTARLA EN EL TEXT Y LA BARRA
        healthText.text = gameManager.GetCurrentHealth().ToString() + "/" + gameManager.GetMaxHealth().ToString();
        healthBar.SetHealth(gameManager.GetCurrentHealth());


        if (!vulnerable && Time.time - invulnerableInstant >= invulnerabilityTime)
        {
            vulnerable = true;

            Color colorSprite = spRd.material.color;
            colorSprite.a = 1f;
            spRd.material.color = colorSprite;
        }


        // Si se recibe da�o mortal durante una transici�n, aunque la muerte suceda, sucede antes la carga de la escena
        // y en la siguiente escena el Hunter aparece con la vida que le corresponda (0)
        // �sto sucede porque la posibilidad de morir solamente se estaba evaluando en el instante en que se recib�a da�o,
        // y no de manera generalizada a lo largo del tiempo
        // Con ello, era posible morir durante una transici�n, que la muerte no llegase a efectuarse y estar jugando con vida 0
        // porque las condiciones de muerte solamente se estaba evaluando en el instante en que se recib�a el da�o
        // Soluci�n: evaluaci�n constante de si los puntos de vida actuales son 0 o menos
        if (gameManager.GetCurrentHealth() <= 0)
        {
            Die();
        }
    }

    public void upgradeHealth()
    {
        gameManager.UpgradeHealth();
        healthBar.SetMaxHealth(gameManager.GetMaxHealth());
    }

    public void receiveHeal()
    {
        gameManager.ReceiveHeal();
    }

    public IEnumerator fullRestoration()
    {
        while (gameManager.GetCurrentHealth() < gameManager.GetMaxHealth())
        {
            gameManager.PlusOneHealth();
            healthBar.SetHealth(gameManager.GetCurrentHealth());
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void TakeDamage(int damage)
    {
        damageSound.Play();
        makeInvulnerable();


        // USAR LOS SETTERS DEL GAME MANAGER
        if (gameManager.GetCurrentHealth() - damage >= 0)
        {
            gameManager.ReceiveDamage(damage);
        } else
        {
            gameManager.SetZeroHealth();
            healthBar.SetHealth(0);
        }


        if (gameManager.GetCurrentHealth() <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.1f);
    }


    public bool isVulnerable()
    {
        return this.vulnerable;
    }


    private void makeInvulnerable()
    {
        vulnerable = false;
        invulnerableInstant = Time.time;

        Color colorSprite = spRd.material.color;
        colorSprite.a = 0.75f;
        spRd.material.color = colorSprite;
    }
}
