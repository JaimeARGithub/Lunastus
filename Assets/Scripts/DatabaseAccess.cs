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


    // Para el acceso a MongoDB
    private const string MONGO_URI = "mongodb+srv://hajithehunter:tqh7sFSo23071995@lunastus.xk1w74t.mongodb.net/?retryWrites=true&w=majority&appName=Lunastus";
    private const string DATABASE_NAME = "jaime";
    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<BsonDocument> saveFilesCollection;
    private IMongoCollection<BsonDocument> playersCollection;


    private List<string> playersList;



    // Start is called before the first frame update
    void Start()
    {
        // Para que el objeto persista a lo largo del juego
        databaseAccess = GameObject.Find("DatabaseAccess");
        DontDestroyOnLoad(databaseAccess);
        //-------------------------------------------------

    
        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);


        playersList = new List<string>();
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
                                            { "currentScene", "Pruebas" },
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
                                            { "augmHealth1", false },
                                            { "augmHealth2", false },
                                            { "augmHealth3", false },
                                            { "augmHealth4", false },
                                            { "augmHealth5", false },
                                            { "augmHealth6", false },
                                            { "augmMissiles1", false },
                                            { "augmMissiles2", false },
                                            { "augmMissiles3", false },
                                            { "augmMissiles4", false }};

        saveFilesCollection.InsertOne(saveFile);
    }


    // Método para el cargado de datos; se pasa por parámetro una string con un nombre,
    // que será el nombre de jugador almacenado en las PlayerPrefs
    // Se usa dicho nombre para buscar entre los archivos guardados y se devuelve el
    // documento Bson completo; luego en el GameManager se trabaja con sus valores
    public BsonDocument GetPlayerData(string playerName)
    {
        saveFilesCollection = db.GetCollection<BsonDocument>("SaveFiles");
        var filter = Builders<BsonDocument>.Filter.Eq("playerName", playerName);
        return saveFilesCollection.Find(filter).FirstOrDefault();
    }
}
