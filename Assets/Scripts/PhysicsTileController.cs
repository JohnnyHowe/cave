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
    float gradient;

    void Update()
    {
        gradient = -Mathf.Tan(cutoffDisplay.eulerAngles.z / (180 / Mathf.PI));
        DropTiles();
        cutoffDisplay.position = Vector3.right * currentCutoff;

        currentCutoff += Time.deltaTime * cutoffSpeed;
        currentCutoff = Mathf.Max(currentCutoff, player.position.x - minPlayerDistance);
    }

    void DropTiles() {
        DropTiles(tileContainer);
    }

    bool ShouldDrop(Transform tile) {
        // To prevent big numbers
        if (gradient > 999) { return tile.transform.position.x < currentCutoff; }
        
        float cutoffAtHeight = cutoffDisplay.position.x + gradient * tile.position.y;
        return tile.position.x < cutoffAtHeight;
    }

    void DropTiles(Transform container)
    {
        foreach (Transform child in container)
        {
            if (ShouldDrop(child))
            {
                if (child.TryGetComponent(out SolidTile tile)) {
                    tile.Drop();
                }
            }
            DropTiles(child);
        }
    }
}
