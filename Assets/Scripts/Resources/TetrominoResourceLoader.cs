using UnityEngine;
using System.Linq;

public static class TetrominoResourceLoader
{
    public static int[,] LoadTetrominoBlueprint(string path)
    {
        var typeSpec = Resources.Load<TextAsset>(path);
        var lines = typeSpec.text.Replace("\r", "").Split('\n')
            .Where(s => s.Length > 0)
            .ToArray();

        var height = lines.Length;
        var width = lines[0].Length;

        var blueprint = new int[width, height];
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {

                blueprint[x, y] = (int)char.GetNumericValue(lines[y][x]);
            }
        }
        return blueprint;
    }
}
