using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Runtime.CompilerServices;



public class DatabaseAccess : MonoBehaviour
{
    // Para que el objeto persista a lo largo del juego
    private GameObject databaseAccess;
    private GameManager gameManager;


    // Para el acceso a MongoDB
    private const string MONGO_URI = "mongodb+srv://hajithehunter:tqh7sFSo23071995@lunastus.xk1w74t.mongodb.net/?retryWrites=true&w=majority&appName=Lunastus";
    private const string DATABASE_NAME = "jaime";
    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<BsonDocument> saveFilesCollection;
    private IMongoCollection<BsonDocument> playersCollection;



    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();


        // Para que el objeto persista a lo largo del juego
        databaseAccess = GameObject.Find("DatabaseAccess");
        DontDestroyOnLoad(databaseAccess);
        //-------------------------------------------------

    
        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);
    }

    public bool PlayerExists(string name)
    {
        bool exists = false;

        // Se recupera la colección con los nombres de jugadores
        playersCollection = db.GetCollection<BsonDocument>("Players");


        // Se recorre la colección y se recupera el campo playerName, para compararlo
        // con el nombre pasado por parámetro
        // Si existe alguna coincidencia, se devuelve un true; si no, un false
        var cursor = playersCollection.FindSync(Builders<BsonDocument>.Filter.Empty);
        foreach (var document in cursor.ToEnumerable())
        {
            var playerName = document["playerName"].AsString;
            if (playerName.Equals(name))
            {
                Debug.Log("EL NOMBRE DEL JUGADOR EXISTE EN LA BBDD");
                exists = true;
                break;
            }
        }

        return exists;
    }


    // Método para ejecutar el PRIMER guardado, nada más generar un nombre en las PlayerPrefs
    // Se establecen todos los valores a los que debe haber por defecto y se escribe en SaveFiles
    // el documento correspondiente por primera vez
    public void FirstDataSave()
    {
        saveFilesCollection = db.GetCollection<BsonDocument>("SaveFiles");


        var saveFile = new BsonDocument { { "playerName", PlayerPrefs.GetString("PlayerName")},
                                            { "currentScene", "InitialScene" },
                                            { "maxHealth", 100 },
                                            { "backdashUnlocked", false },
                                            { "missileUnlocked", false },
                                            { "doublejumpUnlocked", false },
                                            { "currentMissiles", 0 },
                                            { "maxMissiles", 0 },
                                            { "braptor1killed", false },
                                            { "braptor2killed", false },
                                            { "braptor3killed", false },
                                            { "braptor4killed", false },
                                            { "braptor5killed", false },
                                            { "braptor6killed", false },
                                            { "badEnding", false },
                                            { "augmHealth1", false },
                                            { "augmHealth2", false },
                                            { "augmHealth3", false },
                                            { "augmHealth4", false },
                                            { "augmHealth5", false },
                                            { "augmHealth6", false },
                                            { "augmMissiles1", false },
                                            { "augmMissiles2", false },
                                            { "augmMissiles3", false },
                                            { "augmMissiles4", false },
                                            { "firstSave", false },
                                            { "dialogue1triggered", false },
                                            { "dialogue2triggered", false },
                                            { "dialogue3triggered", false },
                                            { "dialogue4triggered", false },
                                            { "dialogue5triggered", false },
                                            { "dialogue6triggered", false }};

        saveFilesCollection.InsertOne(saveFile);
    }


    // Método para el cargado de datos; se utiliza como nombre-filtro el nombre de
    // jugador almacenado en las PlayerPrefs
    // Se recupera la colección de archivos de guardado
    // Se usa dicho nombre para buscar entre los archivos guardados y se devuelve el
    // documento Bson completo; luego en el GameManager se trabaja con sus valores
    public BsonDocument LoadPlayerData()
    {
        saveFilesCollection = db.GetCollection<BsonDocument>("SaveFiles");
        var filter = Builders<BsonDocument>.Filter.Eq("playerName", PlayerPrefs.GetString("PlayerName"));
        return saveFilesCollection.Find(filter).FirstOrDefault();
    }


    // Método para el guardado de datos durante la partda
    // Se recupera toda la colección de archivos de guardado y se usa como nombre-filtro el
    // nombre de jugador almacenado en las PlayerPrefs
    // Se usa dicho nombre para buscar entre los archivos guardados y se ubica el que corresponda
    // Tras ello, se ajustan sus valores según getters del GameManager y se reemplaza el documento en la colección
    public void SavePlayerData()
    {
        saveFilesCollection = db.GetCollection<BsonDocument>("SaveFiles");
        var filter = Builders<BsonDocument>.Filter.Eq("playerName", PlayerPrefs.GetString("PlayerName"));
        var playerData = saveFilesCollection.Find(filter).FirstOrDefault();

        if (playerData != null)
        {
            playerData["currentScene"] = gameManager.GetCurrentScene();

            playerData["maxHealth"] = gameManager.GetMaxHealth();

            playerData["backdashUnlocked"] = gameManager.GetBackdashUnlocked();
            playerData["missileUnlocked"] = gameManager.GetMissileUnlocked();
            playerData["doublejumpUnlocked"] = gameManager.GetDoublejumpUnlocked();

            playerData["currentMissiles"] = gameManager.GetCurrentMissiles();
            playerData["maxMissiles"] = gameManager.GetMaxMissiles();

            playerData["braptor1killed"] = gameManager.GetBraptor1Killed();
            playerData["braptor2killed"] = gameManager.GetBraptor2Killed();
            playerData["braptor3killed"] = gameManager.GetBraptor3Killed();
            playerData["braptor4killed"] = gameManager.GetBraptor4Killed();
            playerData["braptor5killed"] = gameManager.GetBraptor5Killed();
            playerData["braptor6killed"] = gameManager.GetBraptor6Killed();
            playerData["badEnding"] = gameManager.GetBadEnding();

            playerData["augmHealth1"] = gameManager.GetAugmHealth1();
            playerData["augmHealth2"] = gameManager.GetAugmHealth2();
            playerData["augmHealth3"] = gameManager.GetAugmHealth3();
            playerData["augmHealth4"] = gameManager.GetAugmHealth4();
            playerData["augmHealth5"] = gameManager.GetAugmHealth5();
            playerData["augmHealth6"] = gameManager.GetAugmHealth6();

            playerData["augmMissiles1"] = gameManager.GetAugmMissiles1();
            playerData["augmMissiles2"] = gameManager.GetAugmMissiles2();
            playerData["augmMissiles3"] = gameManager.GetAugmMissiles3();
            playerData["augmMissiles4"] = gameManager.GetAugmMissiles4();

            playerData["dialogue1triggered"] = gameManager.GetDialogue1Triggered();
            playerData["dialogue2triggered"] = gameManager.GetDialogue2Triggered();
            playerData["dialogue3triggered"] = gameManager.GetDialogue3Triggered();
            playerData["dialogue4triggered"] = gameManager.GetDialogue4Triggered();
            playerData["dialogue5triggered"] = gameManager.GetDialogue5Triggered();
            playerData["dialogue6triggered"] = gameManager.GetDialogue6Triggered();

            playerData["firstSave"] = gameManager.GetFirstSave();


            // Tras haberse ajustado todo lo necesario del Bson, se reemplaza
            // el documento antiguo por el actualizado
            saveFilesCollection.ReplaceOne(filter, playerData);
        }
    }
}
