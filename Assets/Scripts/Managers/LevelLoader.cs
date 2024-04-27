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




    public IEnumerator LoadScene(string sceneName)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}
