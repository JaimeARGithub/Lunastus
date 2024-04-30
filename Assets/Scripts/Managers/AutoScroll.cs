using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoScroll : MonoBehaviour
{
    private float speed = 20f;
    private float textPositionBegin = -2600f;
    private float textPositionEnd = 2600f;
    private float textPositionEndResetLimit = 2595f;

    RectTransform gameObjectRectTransform;
    [SerializeField] public TextMeshProUGUI creditsText;


    // Start is called before the first frame update
    void Start()
    {
        gameObjectRectTransform = gameObject.GetComponent<RectTransform>();
        StartCoroutine(AutoScrollText());
    }

    
    private IEnumerator AutoScrollText()
    {
        while (gameObjectRectTransform.localPosition.y < textPositionEnd)
        {
            gameObjectRectTransform.Translate(Vector3.up * speed * Time.deltaTime);

            if (gameObjectRectTransform.localPosition.y > textPositionEndResetLimit)
            {
                gameObjectRectTransform.localPosition = Vector3.up * textPositionBegin;
            }

            yield return null; 
        }
    }
}
