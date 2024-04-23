using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioClip menuMusic;
    public AudioClip initialSceneMusic;
    public AudioClip surfaceMusic;
    public AudioClip cavernsMusic;
    public AudioClip depthsMusic;
    public AudioClip corporationMusic;
    public AudioClip gameOverMusic;

    private AudioSource audioSource;


    private void Awake()
    {
        // En el awake, el primer "despertar" del objeto, se verifica si la instancia del
        // MusicManager es nula; si lo es, se asigna como instancia este mismo objeto, y
        // para evitar errores por instancias previas existentes, si no lo es, se destruye.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // En el start, hacemos que el método OnSceneLoaded se suscriba al evento
        // SceneManager.sceneLoaded, logrando así que dicho método se ejecute cada
        // vez que tiene lugar un evento de carga de escena
        // También dejamos reproduciendo la música del menú
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMenuMusic();
    }



    // Cada vez que se carga una escena, se ejecuta el método OnSceneLoaded.
    // Se detecta el nombre de la escena, y según su nombre (palabra clave definitoria
    // que contendrá), se ejecuta uno u otro método de reproducción.

    // En los métodos de reproducción se ejecuta una comprobación: a la hora de reproducir
    // una pista concreta, la reasignación y reproducción ÚNICAMENTE se hace si esa no era
    // la pista que estaba sonando ya (y si el reproductor no estaba reproduciendo nada);
    // de esa manera, para los casos en los que deba sonar la misma pista a lo largo de varias
    // escenas, la reproducción es continua
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName.Contains("Menu"))
        {
            PlayMenuMusic();
        }
        else if (sceneName.Contains("Pruebas"))
        {
            PlayInitialSceneMusic();
        }
        else if (sceneName.Contains("Surface"))
        {
            PlaySurfaceMusic();
        }
        else if (sceneName.Contains("Caverns"))
        {
            PlayCavernsMusic();
        }
        else if (sceneName.Contains("Depths"))
        {
            PlayDepthsMusic();
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

    public void PlaySurfaceMusic()
    {
        if (audioSource.clip != surfaceMusic || !audioSource.isPlaying)
        {
            audioSource.clip = surfaceMusic;
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

    public void PlayDepthsMusic()
    {
        if (audioSource.clip != depthsMusic || !audioSource.isPlaying)
        {
            audioSource.clip = depthsMusic;
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
}
