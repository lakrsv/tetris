using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino
{
    public bool Placed { get; private set; }

    public TetrominoDefinition.TetrominoType Type => _tetrominoDefinition.Type;

    private TetrominoDefinition _tetrominoDefinition;

    private int _currentSegment = 0;

    private List<int[]> _boardPosition = new List<int[]>();


    public void AddPosition(int[] position)
    {
        _boardPosition.Add(position);
    }

    public List<int[]> GetPositions()
    {
        return _boardPosition;
    }

    public void Place()
    {
        Placed = true;
    }

    public Tetromino(TetrominoDefinition tetrominoDefinition)
    {
        _tetrominoDefinition = tetrominoDefinition;
    }

    public int[,] GetCurrentSegment()
    {
        return _tetrominoDefinition.Blueprint[_currentSegment];
    }
}
