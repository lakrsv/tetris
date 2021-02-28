using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class TetrominoResourceLoader
{
    private const char TETROMINO_SEGMENT_DELIMITER = '-';

    public static List<int[,]> LoadTetrominoBlueprint(string path)
    {
        var typeSpec = Resources.Load<TextAsset>(path);
        var segments = typeSpec.text.Replace("\r", "").Split(TETROMINO_SEGMENT_DELIMITER);

        return segments
            .Select(s => LoadTetrominoBlueprintSegment(s))
            .ToList();
    }

    private static int[,] LoadTetrominoBlueprintSegment(string segment)
    {
        var lines = segment.Split('\n')
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
