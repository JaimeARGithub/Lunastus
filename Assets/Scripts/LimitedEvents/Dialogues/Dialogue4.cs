using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue4 : MonoBehaviour
{
    // Referencia al Game Manager
    private GameManager gameManager;


    // Referencias a UI
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;


    // Contenido del diálogo
    [SerializeField] private string[] speaker;
    [TextArea()]
    [SerializeField] private string[] dialogueSentences;


    // Estado de avance del diálogo mediante variable entera
    [SerializeField] private int progress = 0;


    // AudioSources para los sonidos de apertura, avance y cierre del diálogo
    public AudioSource startSound;
    public AudioSource progressSound;
    public AudioSource finishSound;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueCanvas.activeSelf && Input.GetKeyDown(KeyCode.D))
        {
            progressDialogue();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !gameManager.GetDialogue4Triggered())
        {
            gameManager.SetDialogue4Triggered();


            Time.timeScale = 0f;

            startSound.Play();
            dialogueCanvas.SetActive(true);
            speakerText.text = speaker[progress];
            dialogueText.text = dialogueSentences[progress];
        }
    }


    private void progressDialogue()
    {
        progress++;

        if (progress < speaker.Length)
        {
            progressSound.Play();

            speakerText.text = speaker[progress];
            dialogueText.text = dialogueSentences[progress];
        }
        else
        {
            finishSound.Play();

            dialogueCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
