using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public float miny = -5f;
    void FixedUpdate()
    {
        if (transform.position.y < miny) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } 
    }

    void Update()
    {
        if (Input.GetKeyDown("r")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
