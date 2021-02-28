using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{
    public static int[,] RotateRight(this int[,] matrix)
    {
        return RotateMatrix(matrix, true);
    }

    public static int[,] RotateLeft(this int[,] matrix)
    {
        return RotateMatrix(matrix, false);
    }

    public static int[,] RotateMatrix(int[,] matrix, bool clockwise)
    {
        var width = matrix.GetLength(0);
        var height = matrix.GetLength(1);
        int[,] rotated = new int[height, width];

        if (clockwise)
        {
            for(var i = 0; i < height; ++i)
            {
                for(var j = 0; j < width; ++j)
                {
                    rotated[i, j] = matrix[width - j - 1, i];
                }
            }
        } else
        {
            for (var i = 0; i < height; ++i)
            {
                for (var j = 0; j < width; ++j)
                {
                    rotated[i, j] = matrix[j, height - i - 1];
                }
            }
        }
        return rotated;
    }

    public static string ToIdentity(this int[] array)
    {
        return $"x{array[0]}y{array[1]}";
    }
}
