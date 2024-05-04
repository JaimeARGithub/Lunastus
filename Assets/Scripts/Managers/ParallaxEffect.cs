using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float parallaxMultiplier = 0.5f;
    private float spriteWidth, startPositionX;


    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        startPositionX = transform.position.x;
    }


    void LateUpdate()
    {
        float deltaX = (cameraTransform.position.x - previousCameraPosition.x) * parallaxMultiplier;
        float moveAmountX = cameraTransform.position.x * (1 - parallaxMultiplier);

        transform.Translate(new Vector3(deltaX, 0, 0));
        previousCameraPosition = cameraTransform.position;



        if (moveAmountX > startPositionX + spriteWidth -3)
        {
            transform.Translate(new Vector3(spriteWidth,0,0));
            startPositionX += spriteWidth;
        } else if (moveAmountX < startPositionX - spriteWidth +3)
        {
            transform.Translate(new Vector3(-spriteWidth,0,0));
            startPositionX -= spriteWidth;
        }
    }
}
