using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    private GameManager gameManager;
    public Button continueButton;
    private MusicManager musicManager;



    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        musicManager = FindObjectOfType<MusicManager>();
    }


    private void Update()
    {
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(gameManager.GetFirstSave());
        }
    }


    // Para las cuatro opciones del menú principal
    public void OnButtonPlayMenu()
    {
        musicManager.ClickSound();
        SceneManager.LoadScene("PlayMenu");
    }

    public void OnButtonInstructionsMenu()
    {
        musicManager.ClickSound();
        SceneManager.LoadScene("InstructionsMenu");
    }

    public void OnButtonCreditsMenu()
    {
        musicManager.ClickSound();
        SceneManager.LoadScene("CreditsMenu");
    }

    public void OnButtonQuit()
    {
        musicManager.ClickSound();
        Application.Quit();
    }


    // Para los botones de atrás, y para el que se usará para volver al menú principal desde el juego
    public void OnButtonMainMenu()
    {
        musicManager.ClickSound();
        SceneManager.LoadScene("MainMenu");
    }


    // Para los botones disponibles en la pantalla de juego
    public void OnContinueButton()
    {
        musicManager.ClickSound();
        gameManager.LoadData();
        SceneManager.LoadScene(gameManager.GetSceneToPlay());
    }

    public void OnNewButton()
    {
        musicManager.ClickSound();
        gameManager.SetStartValues();
        SceneManager.LoadScene(gameManager.GetSceneToPlay());
    }
}
