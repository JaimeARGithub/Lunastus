using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject gameManager;

    // Para el nombre de jugador que se grabará en la base de datos
    private const string playerNameKey = "PlayerName";
    private string playerName;

    // Para el nombre de la escena actual
    private string currentScene;

    // Para la vida actual y la vida máxima actual
    private int currentHealth;
    private int maxHealth;

    // Para los desbloqueos de mejoras del traje y los valores de misiles
    private bool backdashUnlocked;
    private bool missileUnlocked;
    private bool doublejumpUnlocked;
    private int currentMissiles;
    private int maxMissiles;

    // Para verificar si se ha acabado con los enemigos únicos
    private bool braptor1killed;
    private bool braptor2killed;
    private bool braptor3killed;
    private bool braptor4killed;
    private bool braptor5killed;
    private bool braptor6killed;

    // Para verificar si se han recogido las mejoras de vida y capacidad de
    // misiles dispersas por el mapa
    private bool augmHealth1;
    private bool augmHealth2;
    private bool augmHealth3;
    private bool augmHealth4;
    private bool augmHealth5;
    private bool augmHealth6;
    private bool augmMissiles1;
    private bool augmMissiles2;
    private bool augmMissiles3;
    private bool augmMissiles4;




    // Start is called before the first frame update
    void Start()
    {
        // Lo primero que se hace al abrir el juego es intentar recuperar un nombre
        // de usuario guardado en PlayerPrefs para almacenar datos en MongoDB; si no existe
        // ninguno, se genera uno aleatorio de 6 caracteres usando números, mayúsculas y 
        // minúsculas, y se guarda en las PlayerPrefs.
        playerName = PlayerPrefs.GetString(playerNameKey);
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = GenerateRandomPlayerName(6);
            // Parte de consultar a MongoDB para ver si el nombre generado ya existe, y mientras exista, generar otro
            PlayerPrefs.SetString(playerNameKey, playerName);
            PlayerPrefs.Save();
        }

        Debug.Log("Nombre del jugador: " + playerName);


        gameManager = GameObject.Find("GameManager");
        DontDestroyOnLoad(gameManager);


        SceneManager.LoadScene("Pruebas");
        SetStartValues();
    }


    public void SetStartValues()
    {
        currentScene = "InitialScene";

        maxHealth = 100;
        currentHealth = maxHealth;

        backdashUnlocked = false;
        missileUnlocked = false;
        doublejumpUnlocked = false;
        maxMissiles = 0;
        currentMissiles = maxMissiles;

        braptor1killed = false;
        braptor2killed = false;
        braptor3killed = false;
        braptor4killed = false;
        braptor5killed = false;
        braptor6killed = false;

        augmHealth1 = false;
        augmHealth2 = false;
        augmHealth3 = false;
        augmHealth4 = false;
        augmHealth5 = false;
        augmHealth6 = false;
        augmMissiles1 = false;
        augmMissiles2 = false;
        augmMissiles3 = false;
        augmMissiles4 = false;
    }


    private string GenerateRandomPlayerName(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        System.Random random = new System.Random();
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public int GetMaxHealth()
    {
        return this.maxHealth;
    }

    public int GetCurrentHealth()
    {
        return this.currentHealth;
    }

    public void UpgradeHealth()
    {
        maxHealth += 25; // USAR SETTER DEL GAME MANAGER
        currentHealth = maxHealth; // USAR SETTER DEL GAME MANAGER (meter en el mismo método ampliar vida máxima e iguala valor de actual)
        Debug.Log("LÍMITE DE VIDA: " + maxHealth);
        Debug.Log("VIDA ACTUAL: " + currentHealth);
    }

    public void ReceiveHeal()
    {
        if (currentHealth + 25 <= maxHealth)
        {
            currentHealth += 25;
        }
        else
        {
            currentHealth = maxHealth;
        }

        Debug.Log("LÍMITE DE VIDA: " + maxHealth);
        Debug.Log("VIDA ACTUAL: " + currentHealth);
    }

    public void PlusOneHealth()
    {
        this.currentHealth++;
    }

    public void ReceiveDamage(int damage)
    {
        this.currentHealth -= damage;
    }

    public void SetZeroHealth()
    {
        this.currentHealth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void SetBackdashUnlocked()
    {
        this.backdashUnlocked = true;
        Debug.Log("BACKDASH DESBLOQUEADO");
    }

    public bool GetBackdashUnlocked()
    {
        return this.backdashUnlocked;
    }

    public void SetMissileUnlocked()
    {
        this.missileUnlocked = true;

        maxMissiles += 5;
        currentMissiles = maxMissiles;
        Debug.Log("LÍMITE DE MISILES: " + maxMissiles);
        Debug.Log("MUNICIÓN ACTUAL: " + currentMissiles);
    }

    public void AugmentMissiles()
    {
        if (missileUnlocked)
        {
            maxMissiles += 5;
            currentMissiles = maxMissiles;
            Debug.Log("LÍMITE DE MISILES: " + maxMissiles);
            Debug.Log("MUNICIÓN ACTUAL: " + currentMissiles);
        }
    }

    public void RechargeMissiles()
    {
        if (missileUnlocked)
        {
            if (currentMissiles + 5 <= maxMissiles)
            {
                currentMissiles += 5;
            }
            else
            {
                currentMissiles = maxMissiles;
            }
            Debug.Log("LÍMITE DE MISILES: " + maxMissiles);
            Debug.Log("MUNICIÓN ACTUAL: " + currentMissiles);
        }
    }

    public bool GetMissileUnlocked()
    {
        return this.missileUnlocked;
    }

    public int GetCurrentMissiles()
    {
        return this.currentMissiles;
    }

    public int GetMaxMissiles()
    {
        return this.maxMissiles;
    }

    public void ExpendMissile()
    {
        this.currentMissiles--;
    }

    public void SetDoublejumpUnlocked()
    {
        this.doublejumpUnlocked = true;
        Debug.Log("DOBLE SALTO DESBLOQUEADO");
    }

    public bool GetDoublejumpUnlocked()
    {
        return this.doublejumpUnlocked;
    }

    

    public void SetBraptor1Killed()
    {
        this.braptor1killed = true;
    }

    public bool GetBraptor1Killed()
    {
        return this.braptor1killed;
    }

    public void SetBraptor2Killed()
    {
        this.braptor2killed = true;
    }

    public bool GetBraptor2Killed()
    {
        return this.braptor2killed;
    }

    public void SetBraptor3Killed()
    {
        this.braptor3killed = true;
    }

    public bool GetBraptor3Killed()
    {
        return this.braptor3killed;
    }

    public void SetBraptor4Killed()
    {
        this.braptor4killed = true;
    }

    public bool GetBraptor4Killed()
    {
        return this.braptor4killed;
    }

    public void SetBraptor5Killed()
    {
        this.braptor5killed = true;
    }

    public bool GetBraptor5Killed()
    {
        return this.braptor5killed;
    }

    public void SetBraptor6Killed()
    {
        this.braptor6killed = true;
    }

    public bool GetBraptor6Killed()
    {
        return this.braptor6killed;
    }



    public void SetAugmHealth1()
    {
        this.augmHealth1 = true;
    }

    public bool GetAugmHealth1()
    {
        return this.augmHealth1;
    }

    public void SetAugmHealth2()
    {
        this.augmHealth2 = true;
    }

    public bool GetAugmHealth2()
    {
        return this.augmHealth2;
    }

    public void SetAugmHealth3()
    {
        this.augmHealth3 = true;
    }

    public bool GetAugmHealth3()
    {
        return this.augmHealth3;
    }

    public void SetAugmHealth4()
    {
        this.augmHealth4 = true;
    }

    public bool GetAugmHealth4()
    {
        return this.augmHealth4;
    }

    public void SetAugmHealth5()
    {
        this.augmHealth5 = true;
    }

    public bool GetAugmHealth5()
    {
        return this.augmHealth5;
    }

    public void SetAugmHealth6()
    {
        this.augmHealth6 = true;
    }

    public bool GetAugmHealth6()
    {
        return this.augmHealth6;
    }

    public void SetAugmMissiles1()
    {
        this.augmMissiles1 = true;
    }

    public bool GetAugmMissiles1()
    {
        return this.augmMissiles1;
    }

    public void SetAugmMissiles2()
    {
        this.augmMissiles2 = true;
    }

    public bool GetAugmMissiles2()
    {
        return this.augmMissiles2;
    }

    public void SetAugmMissiles3()
    {
        this.augmMissiles3 = true;
    }

    public bool GetAugmMissiles3()
    {
        return this.augmMissiles3;
    }

    public void SetAugmMissiles4()
    {
        this.augmMissiles4 = true;
    }

    public bool GetAugmMissiles4()
    {
        return this.augmMissiles4;
    }
}
