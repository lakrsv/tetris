using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour, IRenderable
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    private TetrominoDefinition.TetrominoType _tetromino = TetrominoDefinition.TetrominoType.NONE;

    public void Render(Sprites sprites)
    {
        _spriteRenderer.sprite = sprites.GetSpriteForType(_tetromino);
        _spriteRenderer.color = sprites.GetColorForType(_tetromino);
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public float GetPixelsPerUnitSpriteRatio()
    {
        return _spriteRenderer.sprite.rect.width / _spriteRenderer.sprite.pixelsPerUnit;
    }

    public void SetType(TetrominoDefinition.TetrominoType type)
    {
        _tetromino = type;
    }

    public bool HasTetromino()
    {
        return _tetromino != TetrominoDefinition.TetrominoType.NONE;
    }
}
