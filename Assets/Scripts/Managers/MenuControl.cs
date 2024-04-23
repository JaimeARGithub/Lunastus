using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    private GameManager gameManager;
    public Button continueButton;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
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
        SceneManager.LoadScene("PlayMenu");
    }

    public void OnButtonInstructionsMenu()
    {
        SceneManager.LoadScene("InstructionsMenu");
    }

    public void OnButtonCreditsMenu()
    {
        SceneManager.LoadScene("CreditsMenu");
    }

    public void OnButtonQuit()
    {
        Application.Quit();
    }


    // Para los botones de atrás, y para el que se usará para volver al menú principal desde el juego
    public void OnButtonMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    // Para los botones disponibles en la pantalla de juego
    public void OnContinueButton()
    {
        SceneManager.LoadScene(gameManager.GetSceneToPlay());
    }

    public void OnNewButton()
    {
        gameManager.SetStartValues();
        SceneManager.LoadScene(gameManager.GetSceneToPlay());
    }
}
