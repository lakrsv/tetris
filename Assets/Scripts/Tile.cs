using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [SerializeField]
    private Sprite backgroundTile;
    [SerializeField]
    private Sprite blockTile;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void MakeBackgroundTile()
    {
        spriteRenderer.sprite = backgroundTile;
    }

    public void MakeBlockTile()
    {
        spriteRenderer.sprite = blockTile;
    }

    public float GetPixelsPerUnitSpriteRatio()
    {
        return spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit;
    }
}
