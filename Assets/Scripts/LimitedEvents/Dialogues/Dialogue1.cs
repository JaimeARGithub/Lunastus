using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Dialogue1 : MonoBehaviour
{
    // Referencia al Game Manager para detectar el accionamiento del diálogo
    private GameManager gameManager;


    // Referencia al animador del diálogo para animar la imagen
    public Animator animator;


    // Referencias a UI para habilitar canvas y reformular textos
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;


    // Contenido del diálogo (arrays públicos que contienen las strings con los hablantes y las frases)
    [SerializeField] private string[] speaker;
    [TextArea()]
    [SerializeField] private string[] dialogueSentences;


    // Estado de avance del diálogo mediante variable entera
    [SerializeField] private int progress = 0;


    // AudioSources para los sonidos de apertura, avance y cierre del diálogo
    public AudioSource startSound;
    public AudioSource progressSound;
    public AudioSource finishSound;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueCanvas.SetActive(false);

        if (gameManager.GetDialogue1Triggered())
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (dialogueCanvas.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            // A ejecución una vez por frame, SI EL CANVAS DE DIÁLOGO ESTÁ ACTIVO
            // (ya se ha iniciado la conversación) y SE PRESIONA 'F', se avanza el diálogo
            progressDialogue();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // El colisionador establecido hace de activador para el diálogo
        // El diálogo se inicia si no se había reproducido aún (y si lo que choca con el colisionador es el jugador)
        // Nada más iniciarse el diálogo, se settea en el Game Manager para que no se repita

        // Para el texto del hablante, se toma directamente del array de hablantes establecido como serializado
        // Para el texto hablado también, pero en lugar de ponerse tal cual el texto de ese TextMeshPro, se va añadiendo en la corrutina
        if (collision.gameObject.CompareTag("Player") && !gameManager.GetDialogue1Triggered())
        {
            gameManager.SetDialogue1Triggered();

            StartCoroutine(OpenDialogue());

            speakerText.text = speaker[progress];
            StartCoroutine(TypeSentence(dialogueSentences[progress]));
        }
    }


    private void progressDialogue()
    {
        progress++;
        // El StopAllCoroutines se mete porque, si se spammea el botón de avanzar diálogo,
        // las corrutinas se empalman entre ellas y salen mensajes resultantes de frases cruzadas
        StopAllCoroutines();

        if (progress < speaker.Length)
        {
            progressSound.Play();

            speakerText.text = speaker[progress];
            StartCoroutine(TypeSentence(dialogueSentences[progress]));
        } else
        {
            StartCoroutine(CloseDialogue());
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


    // Apertura: se reproduce el sonido, se habilita el canvas, salta la imagen hacia arriba,
    // se espera medio segundo y se detiene el tiempo de juego
    private IEnumerator OpenDialogue()
    {
        startSound.Play();

        dialogueCanvas.SetActive(true);
        animator.SetBool("IsOpen", true);

        yield return new WaitForSeconds(0.5f);

        Time.timeScale = 0f;
    }


    // Cierre: se reproduce el sonido, se reanuda el tiempo de juego, salta la imagen hacia abajo,
    // se espera un segundo y se deshabilita el canvas
    private IEnumerator CloseDialogue()
    {
        finishSound.Play();

        Time.timeScale = 1f;

        animator.SetBool("IsOpen", false);

        yield return new WaitForSeconds(1f);

        dialogueCanvas.SetActive(false);

        // La destrucción del objeto se realiza al final de la corrutina para que el sonido de cierre se reproduzca
        // Si se hace de seguido con la ejecución de la corrutina, las dos cosas se ejecutan casi a la vez
        Destroy(gameObject);
    }
}
