using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    // Referencias a UI
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;


    // Contenido del diálogo
    [SerializeField] private string[] speaker;
    [TextArea()]
    [SerializeField] private string[] dialogueSentences;



    // Update is called once per frame
    void Update()
    {
        
    }
}
