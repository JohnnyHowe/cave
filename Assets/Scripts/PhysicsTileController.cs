using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTileController : MonoBehaviour
{
    public float currentCutoff = 0;
    public float cutoffSpeed = 1f;
    public Transform tileContainer;
    public Transform cutoffDisplay;
    public Transform player;
    public float minPlayerDistance = 1f;

    void Update()
    {
        DropTiles();
        if (cutoffDisplay) { cutoffDisplay.position = Vector3.right * currentCutoff; }

        currentCutoff += Time.deltaTime * cutoffSpeed;
        currentCutoff = Mathf.Max(currentCutoff, player.position.x - minPlayerDistance);
    }

    void DropTiles()
    {
        foreach (Transform child in tileContainer)
        {
            if (child.position.x < currentCutoff)
            {
                child.GetComponent<SolidTile>().Drop();
            }
        }
    }
}
