using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites : MonoSingleton<Sprites>
{
    [SerializeField]
    private Sprite _backgroundSprite;
    [SerializeField]
    private Sprite _tetrominoSprite;

    public Sprite GetSpriteForType(TetrominoDefinition.TetrominoType type)
    {
        switch (type)
        {
            case TetrominoDefinition.TetrominoType.NONE:
                return _backgroundSprite;
            default:
                return _tetrominoSprite;
        }
    }

    public Color GetColorForType(TetrominoDefinition.TetrominoType type)
    {
        switch (type)
        {
            case TetrominoDefinition.TetrominoType.NONE:
                return Color.white;
            default:
                return Color.blue;
        }
    }
}
