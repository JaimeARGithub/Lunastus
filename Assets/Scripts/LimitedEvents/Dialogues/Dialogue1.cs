using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Dialogue1 : MonoBehaviour
{
    // Referencia al Game Manager
    private GameManager gameManager;


    // Referencias a UI
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;


    // Contenido del di�logo
    [SerializeField] private string[] speaker;
    [TextArea()]
    [SerializeField] private string[] dialogueSentences;


    // Estado de avance del di�logo mediante variable entera
    [SerializeField] private int progress = 0;


    // AudioSources para los sonidos de apertura, avance y cierre del di�logo
    public AudioSource startSound;
    public AudioSource progressSound;
    public AudioSource finishSound;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueCanvas.SetActive(false);
    }


    private void Update()
    {
        if (dialogueCanvas.activeSelf && Input.GetKeyDown(KeyCode.D))
        {
            // A ejecuci�n una vez por frame, SI EL CANVAS DE DI�LOGO EST� ACTIVO
            // (ya se ha iniciado la conversaci�n) y SE PRESIONA 'D', se avanza el di�logo
            progressDialogue();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // El colisionador establecido hace de activador para el di�logo
        // El di�logo se inicia si no se hab�a reproducido a�n (y si lo que choca con el colisionador es el jugador)
        // Nada m�s iniciarse el di�logo, se settea en el Game Manager para que no se repita


        // Al iniciarse el di�logo, se detiene el tiempo de juego
        // Se reproduce el sonido de play Y se activa el canvas con el di�logo


        // Para el texto del hablante, se toma directamente del array de hablantes establecido como serializado
        // Para el texto hablado tambi�n, pero en lugar de ponerse tal cual el texto de ese TextMeshPro, se va a�adiendo en la corrutina
        if (collision.gameObject.CompareTag("Player") && !gameManager.GetDialogue1Triggered())
        {
            gameManager.SetDialogue1Triggered();


            Time.timeScale = 0f;
            startSound.Play();
            dialogueCanvas.SetActive(true);


            speakerText.text = speaker[progress];
            StartCoroutine(TypeSentence(dialogueSentences[progress]));
        }
    }


    private void progressDialogue()
    {
        progress++;
        // El StopAllCoroutines se mete porque, si se spammea el bot�n de avanzar di�logo,
        // las corrutinas se empalman entre ellas y salen mensajes resultantes de frases cruzadas
        StopAllCoroutines();

        if (progress < speaker.Length)
        {
            progressSound.Play();

            speakerText.text = speaker[progress];
            StartCoroutine(TypeSentence(dialogueSentences[progress]));
        } else
        {
            finishSound.Play();

            dialogueCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
    }


    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            // yield return null hace una espera de UN frame
            yield return null;
        }
    }
}
