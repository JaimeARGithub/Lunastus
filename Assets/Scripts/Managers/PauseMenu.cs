using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // La variable se hace pública para que de ella puedan leer Combat y Movement del cazador
    // Ésto se hace porque, aunque el juego esté pausado, los intentos de acciones se registran
    // y ejecutan cuando termina la pausa del juego; se mete todo lo que puede hacerse como
    // condicional de que el juego NO esté pausado para evitarlo
    public static bool gamePaused = false;

    // Por encontrarse el sonido de click en el music manager, la reproducción queda a cargo del mismo
    private MusicManager musicManager;

    // Para los sonidos de pausa y reanudar juego, se hace una migración de la reproducción de éstos
    // desde el music manager hasta esta clase, evitando así cargar el music manager con trabajo que no
    // le corresponde; su trabajo queda ceñido a la reproducción de música entre escenas y a la reproducción
    // del sonido de click entre menús, por no reproducirse éste bien de otra manera al cambiar muy rápido
    // las escenas de menús
    public AudioSource pauseSound;
    public AudioSource resumeSound;


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
        pauseSound.Play();

        gamePaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        resumeSound.Play();

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
