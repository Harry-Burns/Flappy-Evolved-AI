using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    public float speed { get; set; }
    void Awake()
    {
        speed = -0.05f;
        Destroy(gameObject, 15f);
    }

    void FixedUpdate()
    {
        this.transform.position += new Vector3(speed, 0, 0);
    }
}
