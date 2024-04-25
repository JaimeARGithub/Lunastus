using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool gamePaused = false;
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

    public void LoadMenu()
    {
        musicManager.ClickSound();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
