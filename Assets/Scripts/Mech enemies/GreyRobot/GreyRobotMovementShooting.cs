using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyRobotMovementShooting : MonoBehaviour
{
    private GameObject hunter;
    private float distance;


    // Start is called before the first frame update
    void Start()
    {
        hunter = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
