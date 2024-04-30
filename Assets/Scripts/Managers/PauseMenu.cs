using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // La variable se hace p�blica para que de ella puedan leer Combat y Movement del cazador
    // �sto se hace porque, aunque el juego est� pausado, los intentos de acciones se registran
    // y ejecutan cuando termina la pausa del juego; se mete todo lo que puede hacerse como
    // condicional de que el juego NO est� pausado para evitarlo
    public static bool gamePaused = false;

    // Por encontrarse el sonido de click en el music manager, la reproducci�n queda a cargo del mismo
    private MusicManager musicManager;

    // Para los sonidos de pausa y reanudar juego, se hace una migraci�n de la reproducci�n de �stos
    // desde el music manager hasta esta clase, evitando as� cargar el music manager con trabajo que no
    // le corresponde; su trabajo queda ce�ido a la reproducci�n de m�sica entre escenas y a la reproducci�n
    // del sonido de click entre men�s, por no reproducirse �ste bien de otra manera al cambiar muy r�pido
    // las escenas de men�s
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
    // al volver a entrar al juego tras Pausa > Men� > Continuar, se comporta
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
