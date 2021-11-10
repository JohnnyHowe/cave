using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class BackgroundParallax : MonoBehaviour
{
    Vector2 camOrigin;
    float tileWidth;
    float xOffset = 0;

    void Awake()
    {
        camOrigin = Camera.main.transform.position;
        tileWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Start()
    {
        CreateChild();
    }

    void CreateChild()
    {
        GameObject child = Instantiate(gameObject, transform);
        Destroy(child.GetComponent<BackgroundParallax>());
        child.transform.position -= Vector3.right * tileWidth;
    }

    void Update()
    {

        // Vector2 parallaxPosition = ((Vector2) Camera.main.transform.position - camOrigin) * transform.position.z + camOrigin;
        Vector2 parallaxPosition = new Vector2(
            (Camera.main.transform.position.x - camOrigin.x) * transform.position.z + camOrigin.x + xOffset,
            (Camera.main.transform.position.y - camOrigin.y) * transform.position.z + camOrigin.y
        );

        if (parallaxPosition.x + tileWidth / 2 <= ScreenHelper.GameBounds.max.x)
        {
            xOffset += tileWidth;
        }

        transform.position = (Vector3)parallaxPosition + Vector3.forward * transform.position.z;
    }
}
