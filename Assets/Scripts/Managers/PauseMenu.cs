using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // La variable se hace pública para que de ella puedan leer Combat y Movement del cazador
    // Ésto se hace porque, aunque el juego esté pausado, los intentos de acciones se registran
    // y ejecutan cuando termina la pausa del juego
    public static bool gamePaused = false;
    private MusicManager musicManager;


    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                Pause();
            } else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        musicManager.PauseSound();

        gamePaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        musicManager.ResumeSound();

        gamePaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    // Si no le metemos el mismo reajuste de variables que en el Resume,
    // al volver a entrar al juego tras Pausa > Menú > Continuar, se comporta
    // como si el juego siguiera pausado
    public void LoadMenu()
    {
        musicManager.ClickSound();

        gamePaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}
