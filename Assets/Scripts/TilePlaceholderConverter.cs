using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlaceholderConverter : MonoBehaviour
{
    public Transform placeholderParent;
    public Transform tileParent;
    public TileInspectorElement[] tiles;  // We assume this is sorted in ascending size
    public float stubborness = 5;

    void Start()
    {
        foreach (Transform section in placeholderParent)
        {
            FillSection(section);
            section.gameObject.SetActive(false);
        }
    }

    void FillSection(Transform section)
    {
        Vector2 sectionSize = new Vector2((int)section.transform.localScale.x, (int)section.transform.localScale.y);
        int[,] section_array = new int[(int)sectionSize.y, (int)sectionSize.x];
        for (int index = 0; index < tiles.Length; index++)
        {
            TileInspectorElement tile = tiles[index];
            int failed = 0;
            while (failed < stubborness * (index + 1) * (index + 1))
            {
                Vector2 tileSize = tile.size;
                Vector2 attemptPosition = new Vector2(
                    RandInt(0, sectionSize.x - tileSize.x),
                    RandInt(0, sectionSize.y - tileSize.y)
                );
                // Is the area clear?
                bool clear = true;
                for (int dx = 0; dx < tileSize.x; dx++)
                {
                    for (int dy = 0; dy < tileSize.y; dy++)
                    {
                        int x = (int)(attemptPosition.x + dx);
                        int y = (int)(attemptPosition.y + dy);
                        if (0 <= x && 0 <= y && x < sectionSize.x && y < sectionSize.y)
                        {
                            if (section_array[y, x] != 0)
                            {
                                clear = false;
                                break;
                            }
                        }
                        else
                        {
                            clear = false;
                            break;
                        }
                    }
                    if (!clear) break;
                }
                if (!clear)
                {
                    failed++;
                }
                else
                {
                    PlaceTile(tile.tile, attemptPosition + (Vector2)(section.position - section.localScale / 2) + tile.size / 2);
                    for (int dx = 0; dx < tileSize.x; dx++)
                    {
                        for (int dy = 0; dy < tileSize.y; dy++)
                        {
                            section_array[(int)(attemptPosition.y + dy), (int)(attemptPosition.x + dx)] = 1;
                        }
                    }
                }
            }
        }
        // Fill in gaps
        for (int dy = 0; dy < sectionSize.y; dy++)
        {
            for (int dx = 0; dx < sectionSize.x; dx++)
            {
                if (section_array[dy, dx] == 0)
                {
                    PlaceTile(tiles[tiles.Length - 1].tile, new Vector2(dx, dy) + (Vector2)(section.position - section.localScale / 2) + Vector2.one / 2);
                }
            }
        }
    }

    void PlaceTile(GameObject tile, Vector2 position)
    {
        GameObject tileObj = Instantiate(tile, tileParent);
        tileObj.transform.localPosition = position;
    }

    int RandInt(float min, float max)
    {
        return RandInt((int)min, (int)max);
    }

    int RandInt(int min, int max)
    {
        return Mathf.Min(max, Mathf.FloorToInt(Random.Range(min, max + 1)));
    }
}

[System.Serializable]
public struct TileInspectorElement
{
    public GameObject tile;
    public Vector2 size;
}