using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = rotation;
    }
}
