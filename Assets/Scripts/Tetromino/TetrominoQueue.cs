using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TetrominoQueue : MonoBehaviour
{
    [SerializeField]
    private int _lookAhead = 5;

    private List<TetrominoDefinition> _tetrominoDefinitions;
    private Queue<TetrominoDefinition> _tetrominos;

    public Tetromino Dequeue()
    {
        var next = _tetrominos.Dequeue();
        EnqueueRandomTetromino();
        return new Tetromino(next);
    }

    private void Awake()
    {
        LoadTetrominos();
        LoadQueue();
        Debug.Log("Hello World");
    }

    private void LoadTetrominos()
    {
        _tetrominoDefinitions = Enum.GetValues(typeof(TetrominoDefinition.TetrominoType)).Cast<TetrominoDefinition.TetrominoType>()
            .Where(t => t != TetrominoDefinition.TetrominoType.NONE)
            .Select(t => TetrominoDefinition.FromBlueprint(t))
            .ToList();

    }

    private void LoadQueue()
    {
        _tetrominos = new Queue<TetrominoDefinition>(_lookAhead);
        for (var i = 0; i < _lookAhead; ++i)
        {
            EnqueueRandomTetromino();
        }
    }

    private void EnqueueRandomTetromino()
    {
        var next = UnityEngine.Random.Range(0, _tetrominoDefinitions.Count);
        _tetrominos.Enqueue(_tetrominoDefinitions[next]);
    }
}
