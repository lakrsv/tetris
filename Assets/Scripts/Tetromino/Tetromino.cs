using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino
{
    public bool Placed { get; private set; }

    public TetrominoDefinition.TetrominoType Type => _tetrominoDefinition.Type;

    private TetrominoDefinition _tetrominoDefinition;

    private int[,] _currentBlueprint;
    private int[,][] _boardPosition;
    private int[] _pivot;

    public void SetBoardPosition(int[,][] boardPosition)
    {
        _boardPosition = boardPosition;
    }

    public int[,][] GetBoardPosition()
    {
        return _boardPosition;
    }
    
    public int[] GetPivotBoardPosition()
    {
        if(_pivot == null)
        {
            return null;
        }
        return _boardPosition[_pivot[0], _pivot[1]];
    }

    public void Place()
    {
        Placed = true;
    }

    public int[] GetPivot()
    {
        return _pivot;
    }

    public Tetromino(TetrominoDefinition tetrominoDefinition)
    {
        _tetrominoDefinition = tetrominoDefinition;
        SetBlueprint(tetrominoDefinition.Blueprint);

    }

    public int[,] GetBlueprint()
    {
        return _currentBlueprint;
    }

    public void SetBlueprint(int[,] blueprint)
    {
        _currentBlueprint = blueprint;
        _pivot = null;
        for (int x = 0; x < blueprint.GetLength(0); ++x)
        {
            for (int y = 0; y < blueprint.GetLength(1); ++y)
            {
                if (_currentBlueprint[x, y] == 2)
                {
                    _pivot = new int[] { x, y };
                    break;
                }
            }
            if (_pivot != null)
            {
                break;
            }
        }
    }
}
