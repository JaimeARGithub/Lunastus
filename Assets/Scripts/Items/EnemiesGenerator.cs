using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    // Variables para las posiciones de generación de los bots marrones y amarillos
    Vector3[] brownRobotsGenerationPositions = new Vector3[5];
    Vector3[] yellowRobotsGenerationPositions = new Vector3[4];
    Vector3[] redRobotsGenerationPositions = new Vector3[2];

    // Variables para la frecuencia de generación
    private float generationInstant;
    private float generationCooldown = 17.5f;

    // Variables para los robots a generar
    public GameObject brownRobot;
    public GameObject yellowRobot;
    public GameObject redRobot;
    public GameObject greyRobot;


    // Start is called before the first frame update
    void Start()
    {
        InitializeGenerationPositions();

        // En el instante de inicio, se ajusta la variable que marca el ritmo de generación a Time.time
        // Ésto es para que, a la hora de generarse enemigos, no se tome como punto de referencia el tiempo desde que se inició
        // el juego, sino el tiempo desde que se accede a la sala
        // P.Ej.: Si generationInstant se deja a 0f y se llega a la sala habiendo transcurrido más de 17.5 segundos, los enemigos
        // se generan al instante de entrar aunque no haya un GenerateEnemies() en el método Start()
        generationInstant = Time.time;
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
                        Instantiate(greyRobot, yellowRobotsGenerationPositions[i], Quaternion.identity);
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

    private void InitializeGenerationPositions()
    {
        // Asignnación del array de posiciones utilizado para el spawn de los BrownRobot + GreyRobot
        brownRobotsGenerationPositions[0] = transform.position + new Vector3(-1.25f, 0, 0);
        brownRobotsGenerationPositions[1] = transform.position + new Vector3(-0.75f, 0, 0);
        brownRobotsGenerationPositions[2] = transform.position + new Vector3(-0.25f, 0, 0);
        brownRobotsGenerationPositions[3] = transform.position + new Vector3(0.25f, 0, 0);
        brownRobotsGenerationPositions[4] = transform.position + new Vector3(0.75f, 0, 0);


        // Asignación del array de posiciones utilizado para el spawn de los YellowRobot + GreyRobot
        yellowRobotsGenerationPositions[0] = transform.position + new Vector3(-1f, 0, 0);
        yellowRobotsGenerationPositions[1] = transform.position + new Vector3(-0.5f, 0, 0);
        yellowRobotsGenerationPositions[2] = transform.position;
        yellowRobotsGenerationPositions[3] = transform.position + new Vector3(0.5f, 0, 0);


        // Asignación del array de posiciones utilizado para el spawn de los RedRobot
        redRobotsGenerationPositions[0] = transform.position + new Vector3(-0.35f, 0, 0);
        redRobotsGenerationPositions[1] = transform.position + new Vector3(0.35f, 0, 0);
    }
}
