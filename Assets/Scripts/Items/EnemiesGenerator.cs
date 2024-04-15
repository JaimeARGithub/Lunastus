using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    // Variables para las posiciones de generación de los bots marrones y amarillos
    Vector3[] brownRobotsGenerationPositions;
    Vector3[] yellowRobotsGenerationPositions;

    // Variables para la frecuencia de generación
    private float generationInstant = 0f;
    private float generationCooldown = 10f;

    // Variables para los robots a generar
    public GameObject brownRobot;
    public GameObject yellowRobot;
    public GameObject redRobot;
    public GameObject greyRobot;


    // Start is called before the first frame update
    void Start()
    {
        // Asignnación del array de posiciones utilizado para el spawn de los BrownRobot
        brownRobotsGenerationPositions = new Vector3[4];
        brownRobotsGenerationPositions[0] = transform.position + new Vector3(-0.75f, 0, 0);
        brownRobotsGenerationPositions[1] = transform.position + new Vector3(-0.25f, 0, 0);
        brownRobotsGenerationPositions[2] = transform.position + new Vector3(0.25f, 0, 0);
        brownRobotsGenerationPositions[3] = transform.position + new Vector3(0.75f, 0, 0);


        // Asignación del array de posiciones utilizado para el spawn de los YellowRobot
        yellowRobotsGenerationPositions = new Vector3[3];
        yellowRobotsGenerationPositions[0] = transform.position + new Vector3(-0.5f, 0, 0);
        yellowRobotsGenerationPositions[1] = transform.position;
        yellowRobotsGenerationPositions[2] = transform.position + new Vector3(0.5f, 0, 0);


        // Generación de enemigos inmediata
        GenerateEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - generationInstant >= generationCooldown)
        {
            GenerateEnemies();
            generationInstant = Time.time;
        }
    }


    private void GenerateEnemies()
    {
        int random = Random.Range(1, 101); //1 incluido, 101 excluido

        if (random <= 35)
        {
            for (int i = 0; i < brownRobotsGenerationPositions.Length; i++)
            {
                Instantiate(brownRobot, brownRobotsGenerationPositions[i], Quaternion.identity);
            }
        } else if (random > 35 && random <= 65)
        {
            for (int i = 0; i < yellowRobotsGenerationPositions.Length; i++)
            {
                Instantiate(yellowRobot, yellowRobotsGenerationPositions[i], Quaternion.identity);
            }
        } else if (random > 65 && random <= 85)
        {
            Instantiate(redRobot, transform.position, Quaternion.identity);
        } else
        {
            Instantiate(greyRobot, transform.position, Quaternion.identity);
        }
    }
}
