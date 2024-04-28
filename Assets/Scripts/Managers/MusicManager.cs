using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioClip menuMusic;
    public AudioClip initialSceneMusic;
    public AudioClip cavernsMusic;
    public AudioClip iceMusic;
    public AudioClip depthsMusic;
    public AudioClip alienBossMusic;
    public AudioClip corporationMusic;
    public AudioClip gameOverMusic;

    // AudioSource para la reproducci�n de m�sica por secciones
    public AudioSource audioSource;
    // AudioSource para la reproducci�n del sonido de click
    public AudioSource clickSound;
    // AudioSources para la reproducci�n de los sonidos de pausar y reanudar juego
    public AudioSource pauseSound;
    public AudioSource resumeSound;



    private void Awake()
    {
        // En el awake, el primer "despertar" del objeto, se verifica si la instancia del
        // MusicManager es nula; si lo es, se asigna como instancia este mismo objeto, y
        // para evitar errores por instancias previas existentes, si no lo es, se destruye.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // En el start, hacemos que el m�todo OnSceneLoaded se suscriba al evento
        // SceneManager.sceneLoaded, logrando as� que dicho m�todo se ejecute cada
        // vez que tiene lugar un evento de carga de escena
        // Tambi�n dejamos reproduciendo la m�sica del men�
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMenuMusic();
    }



    // Cada vez que se carga una escena, se ejecuta el m�todo OnSceneLoaded.
    // Se detecta el nombre de la escena, y seg�n su nombre (palabra clave definitoria
    // que contendr�), se ejecuta uno u otro m�todo de reproducci�n.

    // En los m�todos de reproducci�n se ejecuta una comprobaci�n: a la hora de reproducir
    // una pista concreta, la reasignaci�n y reproducci�n �NICAMENTE se hace si esa no era
    // la pista que estaba sonando ya (y si el reproductor no estaba reproduciendo nada);
    // de esa manera, para los casos en los que deba sonar la misma pista a lo largo de varias
    // escenas, la reproducci�n es continua
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName.Contains("Menu"))
        {
            PlayMenuMusic();
        }
        else if (sceneName.Contains("Initial"))
        {
            PlayInitialSceneMusic();
        }
        else if (sceneName.Contains("Caverns") || sceneName.Contains("Pruebas"))
        {
            PlayCavernsMusic();
        }
        else if (sceneName.Contains("Ice"))
        {
            PlayIceMusic();
        }
        else if (sceneName.Contains("Depths"))
        {
            PlayDepthsMusic();
        }
        else if (sceneName.Contains("AlienBoss"))
        {
            PlayAlienBossMusic();
        }
        else if (sceneName.Contains("Corporation"))
        {
            PlayCorporationMusic();
        }
        else if (sceneName.Contains("GameOver"))
        {
            PlayGameOverMusic();
        }
    }


    public void PlayMenuMusic()
    {
        if (audioSource.clip != menuMusic || !audioSource.isPlaying)
        {
            audioSource.clip = menuMusic;
            audioSource.Play();
        }
    }

    public void PlayInitialSceneMusic()
    {
        if (audioSource.clip != initialSceneMusic || !audioSource.isPlaying)
        {
            audioSource.clip = initialSceneMusic;
            audioSource.Play();
        }
    }

    public void PlayCavernsMusic()
    {
        if (audioSource.clip != cavernsMusic || !audioSource.isPlaying)
        {
            audioSource.clip = cavernsMusic;
            audioSource.Play();
        }
    }

    public void PlayIceMusic()
    {
        if (audioSource.clip != iceMusic || !audioSource.isPlaying)
        {
            audioSource.clip = iceMusic;
            audioSource.Play();
        }
    }

    public void PlayDepthsMusic()
    {
        if (audioSource.clip != depthsMusic || !audioSource.isPlaying)
        {
            audioSource.clip = depthsMusic;
            audioSource.Play();
        }
    }

    public void PlayAlienBossMusic()
    {
        if (audioSource.clip != alienBossMusic || !audioSource.isPlaying)
        {
            audioSource.clip = alienBossMusic;
            audioSource.Play();
        }
    }

    public void PlayCorporationMusic()
    {
        if (audioSource.clip != corporationMusic || !audioSource.isPlaying)
        {
            audioSource.clip = corporationMusic;
            audioSource.Play();
        }
    }

    public void PlayGameOverMusic()
    {
        if (audioSource.clip != gameOverMusic || !audioSource.isPlaying)
        {
            audioSource.clip = gameOverMusic;
            audioSource.Play();
        }
    }


    // M�todos para que la reproducci�n de sonidos en men�s est�
    // concentrada en el MusicManager
    public void ClickSound()
    {
        this.clickSound.Play();
    }

    public void PauseSound()
    {
        this.pauseSound.Play();
    }

    public void ResumeSound()
    {
        this.resumeSound.Play();
    }
}
