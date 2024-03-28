using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimatedObjectShort : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", 0.4f);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
