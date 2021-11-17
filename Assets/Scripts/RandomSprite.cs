using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour
{
    public Sprite[] sprites;
    public float maxRedVariation;
    public float maxGreenVariation;
    public float maxBlueVariation;

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Utils.MoreRandom.RandomChoice(sprites);

        Color color = renderer.color;
        color.r += maxRedVariation * Random.value;
        color.b += maxGreenVariation * Random.value;
        color.g += maxBlueVariation * Random.value;
        renderer.color = color;
    }
}
