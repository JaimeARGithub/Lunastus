using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject gameManager;
    private DatabaseAccess databaseAccess;

    // Para el nombre de jugador que se grabará en la base de datos
    private const string playerNameKey = "PlayerName";
    private string playerName;

    // Para el nombre de la escena actual (en el guardado)
    // y de la escena a jugar (en el cargado)
    private string currentScene;
    private string sceneToPlay;

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

    // Variable para elegir el final del juego, en función de si se mataron o no los seis biceraptors
    private bool badEnding;

    // Variable para verificar si se ha ejecutado el primer guardado ingame (y mostrar o no la opción continuar)
    private bool firstSave;

    // Variables para verificar (y que no se activen de nuevo) si se han
    // ido activando los diálogos disponibles por el juego
    private bool dialogue1triggered;
    private bool dialogue2triggered;
    private bool dialogue3triggered;
    private bool dialogue4triggered;
    private bool dialogue5triggered;
    private bool dialogue6triggered;




    // Start is called before the first frame update
    void Start()
    {
        databaseAccess = FindObjectOfType<DatabaseAccess>();



        // Lo primero que se hace al abrir el juego es intentar recuperar un nombre
        // de usuario guardado en PlayerPrefs

        // Si no existe ninguno (es la primera vez que se abre el juego), se genera
        // uno aleatorio de 6 caracteres usando números, mayúsculas y minúsculas.

        // Durante la generación, se lee la lista de jugadores existentes en MongoDB;
        // si se ha generado un nombre que YA EXISTE en la lista de jugadores, se
        // genera otro aleatorio.

        // Una vez se guarda el nombre en las PlayersPrefs, se crea también el archivo
        // de guardado en MongoDB.


        // El archivo de guardado se escribe en el preciso instante en que se elige un
        // nombre para evitar problemas; si solamente se guarda al tocar una estación
        // de guardado, al ser datos no almacenados en local, existe la posibilidad de que
        // en dos partidas al mismo tiempo:
        // 1.- Se lea de la lista de jugadores
        // 2.- Se genere un nombre que NO exista en ella, pero que COINCIDA entre las dos partidas
        // 3.- A la hora de guardar la partida, al identificarse el archivo mediante el nombre del
        // jugador, que uno vaya sobreescribiendo al otro
        playerName = PlayerPrefs.GetString(playerNameKey);

        // SI ES EL PRIMER ACCESO AL JUEGO:
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = GenerateRandomPlayerName(6);


            while (databaseAccess.PlayerExists(playerName))
            {
                playerName = GenerateRandomPlayerName(6);
            }


            PlayerPrefs.SetString(playerNameKey, playerName);
            PlayerPrefs.Save();

            SetStartValues();
            databaseAccess.FirstDataSave();
            
        } else
        {
            // INTENTO DE DECUPERACIÓN DE DATOS SI EL PLAYERNAME YA EXISTE
            Debug.Log("EL JUGADOR EXISTE");
        }

        Debug.Log("Nombre del jugador: " + playerName);
        LoadData();

        //PlayerPrefs.DeleteAll();




        // Persistencia del GameManager durante el juego
        gameManager = GameObject.Find("GameManager");
        DontDestroyOnLoad(gameManager);
        //----------------------------------------------

        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }


    // Para determinar qué final se alcanza
    public void SetBadEnding()
    {
        this.badEnding = true;
        Debug.Log("ACTIVADO EL BAD ENDING");
    }

    public bool GetBadEnding()
    {
        return this.badEnding;
    }

    public bool CheckBadEnding()
    {
        bool checker = false;

        if (GetBraptor1Killed() && GetBraptor2Killed() && GetBraptor3Killed() &&
                GetBraptor4Killed() && GetBraptor5Killed() && GetBraptor6Killed() &&
                !GetBadEnding())
        {
            checker = true;
        }

        return checker;
    }


    // Para determinar la gestión de escenas; la actual, para los guardados,
    // y la que debe cargarse, para los continuar partida y juego nuevo
    public string GetCurrentScene()
    {
        return this.currentScene;
    }


    public string GetSceneToPlay()
    {
        return this.sceneToPlay;
    }


    // MÉTODO PARA SITUAR LOS VALORES INICIALES EN UNA PARTIDA NUEVA
    public void SetStartValues()
    {
        sceneToPlay = "InitialScene";

        firstSave = false;

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
        badEnding = false;

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

        dialogue1triggered = false;
        dialogue2triggered = false;
        dialogue3triggered = false;
        dialogue4triggered = false;
        dialogue5triggered = false;
        dialogue6triggered = false;
    }


    // MÉTODO PARA GENERAR UN NOMBRE DE JUGADOR ALEATORIO
    private string GenerateRandomPlayerName(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        System.Random random = new System.Random();
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    // MÉTODOS PARA LA VIDA DEL JUGADOR
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


    // MÉTODOS PARA EL BACKDASH
    public void SetBackdashUnlocked()
    {
        this.backdashUnlocked = true;
        Debug.Log("BACKDASH DESBLOQUEADO");
    }

    public bool GetBackdashUnlocked()
    {
        return this.backdashUnlocked;
    }


    // MÉTODOS PARA LOS MISILES Y SU MUNICIÓN
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


    // MÉTODOS PARA EL DOBLE SALTO
    public void SetDoublejumpUnlocked()
    {
        this.doublejumpUnlocked = true;
        Debug.Log("DOBLE SALTO DESBLOQUEADO");
    }

    public bool GetDoublejumpUnlocked()
    {
        return this.doublejumpUnlocked;
    }

    
    // MÉTODOS PARA EL ASESINATO PERMANENTE DE LOS BICERAPTOR
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


    // MÉTODOS PARA LA RECOGIDA DE LOS AUMENTOS PERMANENTES DE SALUD
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



    // MÉTODOS PARA LOS AUMENTOS PERMANENTES DE CAPACIDAD DE MISILES
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


    // MÉTODOS GETTERS Y SETTERS PARA LAS CONVERSACIONES DE SUCESO ÚNICO
    public void SetDialogue1Triggered()
    {
        this.dialogue1triggered = true;
    }

    public bool GetDialogue1Triggered ()
    {
        return this.dialogue1triggered;
    }

    public void SetDialogue2Triggered()
    {
        this.dialogue2triggered = true;
    }

    public bool GetDialogue2Triggered()
    {
        return this.dialogue2triggered;
    }

    public void SetDialogue3Triggered()
    {
        this.dialogue3triggered = true;
    }

    public bool GetDialogue3Triggered()
    {
        return this.dialogue3triggered;
    }

    public void SetDialogue4Triggered()
    {
        this.dialogue4triggered = true;
    }

    public bool GetDialogue4Triggered()
    {
        return this.dialogue4triggered;
    }

    public void SetDialogue5Triggered()
    {
        this.dialogue5triggered = true;
    }

    public bool GetDialogue5Triggered()
    {
        return this.dialogue5triggered;
    }

    public void SetDialogue6Triggered()
    {
        this.dialogue6triggered = true;
    }

    public bool GetDialogue6Triggered()
    {
        return this.dialogue6triggered;
    }



    // Métodos para leer y settear la variable del primer guardado
    public void SetFirstSave()
    {
        this.firstSave = true;
    }

    public bool GetFirstSave()
    {
        return this.firstSave;
    }


    // MÉTODO PARA CARGAR DATOS DESDE MONGODB
    // CUANDO EL NOMBRE DEL PLAYERPREFS EXISTE
    public void LoadData()
    {
        BsonDocument playerdata = databaseAccess.LoadPlayerData();

        this.sceneToPlay = playerdata["currentScene"].AsString;

        this.maxHealth = playerdata["maxHealth"].AsInt32;
        this.currentHealth = this.maxHealth;

        this.backdashUnlocked = playerdata["backdashUnlocked"].AsBoolean;
        this.missileUnlocked = playerdata["missileUnlocked"].AsBoolean;
        this.doublejumpUnlocked = playerdata["doublejumpUnlocked"].AsBoolean;

        this.currentMissiles = playerdata["currentMissiles"].AsInt32;
        this.maxMissiles = playerdata["maxMissiles"].AsInt32;

        this.braptor1killed = playerdata["braptor1killed"].AsBoolean;
        this.braptor2killed = playerdata["braptor2killed"].AsBoolean;
        this.braptor3killed = playerdata["braptor3killed"].AsBoolean;
        this.braptor4killed = playerdata["braptor4killed"].AsBoolean;
        this.braptor5killed = playerdata["braptor5killed"].AsBoolean;
        this.braptor6killed = playerdata["braptor6killed"].AsBoolean;

        this.augmHealth1 = playerdata["augmHealth1"].AsBoolean;
        this.augmHealth2 = playerdata["augmHealth2"].AsBoolean;
        this.augmHealth3 = playerdata["augmHealth3"].AsBoolean;
        this.augmHealth4 = playerdata["augmHealth4"].AsBoolean;
        this.augmHealth5 = playerdata["augmHealth5"].AsBoolean;
        this.augmHealth6 = playerdata["augmHealth6"].AsBoolean;

        this.augmMissiles1 = playerdata["augmMissiles1"].AsBoolean;
        this.augmMissiles2 = playerdata["augmMissiles2"].AsBoolean;
        this.augmMissiles3 = playerdata["augmMissiles3"].AsBoolean;
        this.augmMissiles4 = playerdata["augmMissiles4"].AsBoolean;

        this.dialogue1triggered = playerdata["dialogue1triggered"].AsBoolean;
        this.dialogue2triggered = playerdata["dialogue2triggered"].AsBoolean;
        this.dialogue3triggered = playerdata["dialogue3triggered"].AsBoolean;
        this.dialogue4triggered = playerdata["dialogue4triggered"].AsBoolean;
        this.dialogue5triggered = playerdata["dialogue5triggered"].AsBoolean;
        this.dialogue6triggered = playerdata["dialogue6triggered"].AsBoolean;

        this.firstSave = playerdata["firstSave"].AsBoolean;
    }


    public void SaveData()
    {
        databaseAccess.SavePlayerData();
    }


    // PARA EL ESTADO DE GAME OVER AL MORIR
    public void GameOverState()
    {
        SceneManager.LoadScene("GameOver");
    }
}
