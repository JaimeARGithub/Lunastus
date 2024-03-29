using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectLong : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", 0.6f);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
