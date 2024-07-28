using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    // Variables para las posiciones de generaci�n de los bots marrones y amarillos
    Vector3[] brownRobotsGenerationPositions;
    Vector3[] yellowRobotsGenerationPositions;
    Vector3[] redRobotsGenerationPositions;

    // Variables para la frecuencia de generaci�n
    private float generationInstant = 0f;
    private float generationCooldown = 17.5f;

    // Variables para los robots a generar
    public GameObject brownRobot;
    public GameObject yellowRobot;
    public GameObject redRobot;
    public GameObject greyRobot;


    // Start is called before the first frame update
    void Start()
    {
        // Asignnaci�n del array de posiciones utilizado para el spawn de los BrownRobot + GreyRobot
        brownRobotsGenerationPositions = new Vector3[5];
        brownRobotsGenerationPositions[0] = transform.position + new Vector3(-1.25f, 0, 0);
        brownRobotsGenerationPositions[1] = transform.position + new Vector3(-0.75f, 0, 0);
        brownRobotsGenerationPositions[2] = transform.position + new Vector3(-0.25f, 0, 0);
        brownRobotsGenerationPositions[3] = transform.position + new Vector3(0.25f, 0, 0);
        brownRobotsGenerationPositions[4] = transform.position + new Vector3(0.75f, 0, 0);


        // Asignaci�n del array de posiciones utilizado para el spawn de los YellowRobot + GreyRobot
        yellowRobotsGenerationPositions = new Vector3[4];
        yellowRobotsGenerationPositions[0] = transform.position + new Vector3(-1f, 0, 0);
        yellowRobotsGenerationPositions[1] = transform.position + new Vector3(-0.5f, 0, 0);
        yellowRobotsGenerationPositions[2] = transform.position;
        yellowRobotsGenerationPositions[3] = transform.position + new Vector3(0.5f, 0, 0);


        // Asignaci�n del array de posiciones utilizado para el spawn de los RedRobot
        redRobotsGenerationPositions = new Vector3[2];
        redRobotsGenerationPositions[0] = transform.position + new Vector3(-0.35f, 0, 0);
        redRobotsGenerationPositions[1] = transform.position + new Vector3(0.35f, 0, 0);
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
        int random = Random.Range(1, 4); //1 incluido, 4 excluido


        switch (random)
        {
            case 1:
                for (int i = 0; i < brownRobotsGenerationPositions.Length; i++)
                {
                    if (i==0)
                    {
                        Instantiate(greyRobot, brownRobotsGenerationPositions[i], Quaternion.identity);

                    } else
                    {
                        Instantiate(brownRobot, brownRobotsGenerationPositions[i], Quaternion.identity);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < yellowRobotsGenerationPositions.Length; i++)
                {
                    if (i==0)
                    {
                        Instantiate(greyRobot, brownRobotsGenerationPositions[i], Quaternion.identity);
                    } else
                    {
                        Instantiate(yellowRobot, yellowRobotsGenerationPositions[i], Quaternion.identity);
                    }
                }
                break;
            case 3:
                for (int i = 0; i < redRobotsGenerationPositions.Length; i++)
                {
                    Instantiate(redRobot, redRobotsGenerationPositions[i], Quaternion.identity);
                }
                break;
        }
    }
}
