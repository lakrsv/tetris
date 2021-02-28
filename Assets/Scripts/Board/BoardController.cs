using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardController : MonoBehaviour
{
    private static int[] GRAVITY = { 0, -1 };

    [SerializeField]
    private float _stepInterval = 0.25f;
    [SerializeField]
    private float _blockPlaceGracePeriod = 0f;
    [SerializeField]
    private BoardRender _boardRender;
    [SerializeField]
    private TetrominoQueue _tetrominoQueue;

    private Tetromino _currentTetromino;

    public bool DropFast;

    private bool _executeGameLoop = true;

    public bool MoveLeft()
    {
        var direction = new int[] { -1, 0 };
        if (CanMoveTetromino(_currentTetromino, direction))
        {
            MoveTetromino(_currentTetromino, direction);
            _boardRender.Render();
            return true;
        }
        return false;
    }

    public bool MoveRight()
    {
        var direction = new int[] { 1, 0 };
        if (CanMoveTetromino(_currentTetromino, direction))
        {
            MoveTetromino(_currentTetromino, direction);
            _boardRender.Render();
            return true;
        }
        return false;
    }

    public bool DropHard()
    {
        return false;
    }

    public bool RotateLeft()
    {
        return false;
    }

    public bool RotateRight()
    {
        return false;
    }

    private IEnumerator Start()
    {
        // TODO - Use exit condition
        while (_executeGameLoop)
        {
            yield return GameLoop();
        }
    }

    private IEnumerator GameLoop()
    {
        var waitTime = DropFast ? _stepInterval / 2f : _stepInterval;
        yield return new WaitForSeconds(waitTime);

        if (_currentTetromino == null || _currentTetromino.Placed)
        {
            _currentTetromino = _tetrominoQueue.Dequeue();
            if (!SpawnTetromino(_currentTetromino))
            {
                // Lose the game
                Debug.Log("Game lost!");
                _executeGameLoop = false;
                yield break;
            }
        }
        else
        {
            if (!CanMoveTetromino(_currentTetromino, GRAVITY))
            {
                waitTime = DropFast ? _blockPlaceGracePeriod / 2f : _blockPlaceGracePeriod;
                yield return new WaitForSeconds(waitTime);
            }
            ApplyGravityToTetromino(_currentTetromino);
        }

        _boardRender.Render();
    }

    private bool SpawnTetromino(Tetromino tetromino)
    {
        var boardMidpoint = _boardRender.Width / 2;

        var tetrominoTiles = tetromino.GetCurrentSegment();

        int tetrominoWidth = tetrominoTiles.GetLength(0);
        int tetrominoHeight = tetrominoTiles.GetLength(1);

        Debug.Log("Spawning tetromino " + tetromino.Type);

        for (int boardX = boardMidpoint - tetrominoWidth / 2; boardX < boardMidpoint + tetrominoWidth / 2 + 1; ++boardX)
        {
            for (int boardY = _boardRender.Height - 1; boardY > _boardRender.Height - tetrominoHeight - 1; --boardY)
            {
                var tileX = boardX - boardMidpoint + tetrominoWidth / 2;
                var tileY = Mathf.Abs(boardY - _boardRender.Height + 1);

                if (tileX >= tetrominoWidth || tileY >= tetrominoHeight)
                {
                    continue;
                }
                if (tetrominoTiles[tileX, tileY] == 0)
                {
                    continue;
                }

                var tile = _boardRender.Tiles[boardX, boardY];
                if (tile.HasTetromino())
                {
                    return false;
                }
                tile.SetType(tetromino.Type);
                tetromino.AddPosition(new int[] { boardX, boardY });

                Debug.Log($"BoardX: {boardX} BoardY: {boardY} for tileX: {tileX} tileY: {tileY}");
            }
        }
        return true;
    }

    private void MoveTetromino(Tetromino tetromino, int[] direction)
    {
        var positionToUpdate = tetromino.GetPositions();
        foreach (var position in positionToUpdate)
        {
            _boardRender.Tiles[position[0], position[1]].SetType(TetrominoDefinition.TetrominoType.NONE);
            position[0] += direction[0];
            position[1] += direction[1];
        }
        foreach (var position in positionToUpdate)
        {
            _boardRender.Tiles[position[0], position[1]].SetType(tetromino.Type);
        }
    }

    private bool CanMoveTetromino(Tetromino tetromino, int[] direction)
    {
        var currentPositions = tetromino.GetPositions();
        var positionIdentities = new HashSet<string>(currentPositions.Select(p => CreatePositionIdentity(p)).ToList());

        foreach (var position in currentPositions)
        {
            var newX = position[0] + direction[0];
            var newY = position[1] + direction[1];

            if (positionIdentities.Contains(CreatePositionIdentity(new int[] { newX, newY })))
            {
                continue;
            }

            if (newX < 0 || newY < 0 || newX >= _boardRender.Width || newY >= _boardRender.Height)
            {
                return false;
            }

            if (_boardRender.Tiles[newX, newY].HasTetromino())
            {
                return false;
            }
        }
        return true;
    }

    private void ApplyGravityToTetromino(Tetromino tetromino)
    {
        if (!CanMoveTetromino(tetromino, GRAVITY))
        {
            tetromino.Place();
        }
        else
        {
            MoveTetromino(tetromino, GRAVITY);
        }
    }

    private string CreatePositionIdentity(int[] position)
    {
        return $"x{position[0]}y{position[1]}";
    }
}
