using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino
{
    public enum Type
    {
        I,
        O,
        T,
        J,
        L,
        S,
        Z
    }

    private int[,] blueprint;

    public Tetromino(int[,] blueprint)
    {
        this.blueprint = blueprint;
    }

    public static Tetromino FromBlueprint(Type type)
    {
        var typeSpec = Resources.Load<TextAsset>("Tetromino/" + type.ToString());
        var lines = typeSpec.text.Split('\n');

        var height = lines.Length;
        var width = lines[0].Length;


        var blueprint = new int[width, height];
        for(int y = 0; y < height; ++y)
        {
            for(int x = 0; x < width; ++x)
            {

                blueprint[x,y] = (int)char.GetNumericValue(lines[y][x]);
            }
        }
        return new Tetromino(blueprint);
    }
}
