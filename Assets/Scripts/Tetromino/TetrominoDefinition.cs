using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoDefinition
{

    public enum TetrominoType
    {
        NONE = 0,
        I = 1,
        O = 2,
        T = 3,
        J = 4,
        L = 5,
        S = 6,
        Z = 7
    }

    public TetrominoType Type { get; private set; }
    public int[,] Blueprint { get; private set; }


    private TetrominoDefinition(int[,] blueprint, TetrominoType type)
    {
        Blueprint = blueprint;
        Type = type;
    }

    public static TetrominoDefinition FromBlueprint(TetrominoType type)
    {
        var blueprint = TetrominoResourceLoader.LoadTetrominoBlueprint("Tetromino/" + type.ToString());
        return new TetrominoDefinition(blueprint, type);
    }
}
