using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SolidTile : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float baseMass = 0;
    public float sizeMassMultiplier = 10f;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Drop() {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.mass = baseMass + transform.localScale.x * transform.localScale.y * sizeMassMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
