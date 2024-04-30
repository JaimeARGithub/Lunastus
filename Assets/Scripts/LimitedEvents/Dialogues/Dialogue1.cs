using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue1 : MonoBehaviour
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


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueCanvas.SetActive(false);
    }


    private void Update()
    {
        if (dialogueCanvas.activeSelf && Input.GetKeyDown(KeyCode.D))
        {
            progressDialogue();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !gameManager.GetDialogue1Triggered())
        {
            gameManager.SetDialogue1Triggered();


            Time.timeScale = 0f;
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
            speakerText.text = speaker[progress];
            dialogueText.text = dialogueSentences[progress];
        } else
        {
            dialogueCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
