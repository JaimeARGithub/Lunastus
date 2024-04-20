using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Runtime.CompilerServices;



public class DatabaseAccess : MonoBehaviour
{
    // Para que el objeto persista a lo largo del juego
    private GameObject DatabaseItem;


    // Para el acceso a MongoDB
    private const string MONGO_URI = "mongodb+srv://hajithehunter:tqh7sFSo23071995@lunastus.xk1w74t.mongodb.net/?retryWrites=true&w=majority&appName=Lunastus";
    private const string DATABASE_NAME = "jaime";
    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<BsonDocument> saveFilesCollection;



    // Start is called before the first frame update
    void Start()
    {
        // Para que el objeto persista a lo largo del juego
        DatabaseItem = GameObject.Find("Database");
        DontDestroyOnLoad(DatabaseItem);


        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);
        saveFilesCollection = db.GetCollection<BsonDocument>("SaveFiles");

        var document = new BsonDocument { { "playerName", "LUIS" }, { "nombreMascota", "THOR" } };
        saveFilesCollection.InsertOne(document);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
