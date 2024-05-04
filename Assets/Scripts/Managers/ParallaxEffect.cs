using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float parallaxMultiplier = 0.5f;
    private float spriteWidth, startPositionX;
    private float spriteHeight, startPositionY;


    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;


        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        startPositionX = transform.position.x;

        spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        startPositionY = transform.position.y;
    }


    void LateUpdate()
    {
        float deltaX = (cameraTransform.position.x - previousCameraPosition.x) * parallaxMultiplier;
        float deltaY = (cameraTransform.position.y - previousCameraPosition.y) * parallaxMultiplier;
        float moveAmountX = cameraTransform.position.x * (1 - parallaxMultiplier);
        float moveAmountY = cameraTransform.position.y * (1 - parallaxMultiplier);

        transform.Translate(new Vector3(deltaX, deltaY, 0));
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

        if (moveAmountY > startPositionY + spriteHeight - 3)
        {
            transform.Translate(new Vector3(0, spriteHeight, 0));
            startPositionY += spriteHeight;
        }
        else if (moveAmountY < startPositionY - spriteHeight + 3)
        {
            transform.Translate(new Vector3(0, -spriteHeight, 0));
            startPositionY -= spriteHeight;
        }
    }
}
