using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseAccess : MonoBehaviour
{
    private GameObject database;


    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.Find("Database");
        DontDestroyOnLoad(database);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
