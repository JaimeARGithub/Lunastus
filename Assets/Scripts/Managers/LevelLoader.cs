using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // El animator hay que meterlo como public porque el script va asignado al LevelLoader
    // (empty object), y el animator que vamos a utilizar es el de Crossfade, canvas interno
    // creado con las animaciones también creadas
    public Animator animator;

    // Para la lógica sobre qué escena cargar al llegar a AlienBoss
    private GameManager gameManager;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }


    public IEnumerator LoadScene(string sceneName)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        if (sceneName.Equals("AlienBoss"))
        {
            if (gameManager.GetBadEnding())
            {
                SceneManager.LoadScene(sceneName);
            } else
            {
                SceneManager.LoadScene("Depths19");
            }
        } else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
