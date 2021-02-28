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
        var rotatedBlueprint = _currentTetromino.GetBlueprint().RotateLeft();

        var width = rotatedBlueprint.GetLength(0);
        var height = rotatedBlueprint.GetLength(1);

        var boardPivot = _currentTetromino.GetPivotBoardPosition();
        if (boardPivot == null)
        {
            return false;
        }

        var newBoardPosition = new int[width, height][];

        for (int x = 0; x < width; ++x)
        {
            for (int y = height - 1; y >= 0; --y)
            {
                if (rotatedBlueprint[x, y] == 0)
                {
                    continue;
                }

                var blueprintPivot = _currentTetromino.GetPivot();
                newBoardPosition[x, y] = new int[] { x + boardPivot[0] - blueprintPivot[0], Mathf.Abs(y - height + 1) + boardPivot[1] - blueprintPivot[1] };
            }
        }

        if (CanPlaceTetromino(_currentTetromino, newBoardPosition))
        {
            PlaceTetromino(_currentTetromino, newBoardPosition);
            _currentTetromino.SetBlueprint(rotatedBlueprint);
            _boardRender.Render();
            return true;
        }

        return false;
    }

    public bool RotateRight()
    {
        var rotatedBlueprint = _currentTetromino.GetBlueprint().RotateRight();

        var width = rotatedBlueprint.GetLength(0);
        var height = rotatedBlueprint.GetLength(1);

        var boardPivot = _currentTetromino.GetPivotBoardPosition();
        if (boardPivot == null)
        {
            return false;
        }

        var newBoardPosition = new int[width, height][];

        for (int x = 0; x < width; ++x)
        {
            for (int y = height - 1; y >= 0; --y)
            {
                if (rotatedBlueprint[x, y] == 0)
                {
                    continue;
                }

                var blueprintPivot = _currentTetromino.GetPivot();
                newBoardPosition[x, y] = new int[] { x + boardPivot[0] - blueprintPivot[0], Mathf.Abs(y - height + 1) + boardPivot[1] - blueprintPivot[1] };
            }
        }

        if (CanPlaceTetromino(_currentTetromino, newBoardPosition))
        {
            PlaceTetromino(_currentTetromino, newBoardPosition);
            _currentTetromino.SetBlueprint(rotatedBlueprint);
            _boardRender.Render();
            return true;
        }

        return false;
    }

    private IEnumerator Start()
    {
        // TODO - Don't use ienumerator
        while (_executeGameLoop)
        {
            yield return GameLoop();
        }
    }

    private IEnumerator GameLoop()
    {
        var waitTime = DropFast ? _stepInterval / 4f : _stepInterval;
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
                waitTime = DropFast ? _blockPlaceGracePeriod / 4f : _blockPlaceGracePeriod;
                yield return new WaitForSeconds(waitTime);
            }
            ApplyGravityToTetromino(_currentTetromino);
        }

        _boardRender.Render();
    }

    private bool SpawnTetromino(Tetromino tetromino)
    {
        var boardMidpoint = _boardRender.Width / 2;

        var tetrominoTiles = tetromino.GetBlueprint();

        int tetrominoWidth = tetrominoTiles.GetLength(0);
        int tetrominoHeight = tetrominoTiles.GetLength(1);

        Debug.Log("Spawning tetromino " + tetromino.Type);

        var boardPosition = new int[tetrominoWidth, tetrominoHeight][];

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
                boardPosition[tileX, tileY] = new int[] { boardX, boardY };
                Debug.Log($"BoardX: {boardX} BoardY: {boardY} for tileX: {tileX} tileY: {tileY}");
            }
        }

        tetromino.SetBoardPosition(boardPosition);
        return true;
    }

    private void MoveTetromino(Tetromino tetromino, int[] direction)
    {
        var positionToUpdate = tetromino.GetBoardPosition();
        foreach (var position in positionToUpdate)
        {
            if (position == null)
            {
                continue;
            }

            _boardRender.Tiles[position[0], position[1]].SetType(TetrominoDefinition.TetrominoType.NONE);
            position[0] += direction[0];
            position[1] += direction[1];
        }
        foreach (var position in positionToUpdate)
        {
            if (position == null)
            {
                continue;
            }
            _boardRender.Tiles[position[0], position[1]].SetType(tetromino.Type);
        }
    }

    private bool CanMoveTetromino(Tetromino tetromino, int[] direction)
    {
        var currentPositions = tetromino.GetBoardPosition();

        var positionIdentities = new HashSet<string>();
        foreach (var p in currentPositions)
        {
            if (p == null)
            {
                continue;
            }
            positionIdentities.Add(p.ToIdentity());
        }

        foreach (var position in currentPositions)
        {
            if (position == null)
            {
                continue;
            }

            var newX = position[0] + direction[0];
            var newY = position[1] + direction[1];

            if (positionIdentities.Contains(new int[] { newX, newY }.ToIdentity()))
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

    private void PlaceTetromino(Tetromino tetromino, int[,][] boardPosition)
    {
        var positionToUpdate = tetromino.GetBoardPosition();
        foreach (var position in positionToUpdate)
        {
            if (position == null)
            {
                continue;
            }
            _boardRender.Tiles[position[0], position[1]].SetType(TetrominoDefinition.TetrominoType.NONE);
        }
        foreach (var position in boardPosition)
        {
            if (position == null)
            {
                continue;
            }
            _boardRender.Tiles[position[0], position[1]].SetType(tetromino.Type);
        }
        tetromino.SetBoardPosition(boardPosition);
    }

    private bool CanPlaceTetromino(Tetromino tetromino, int[,][] boardPosition)
    {
        var currentPositions = tetromino.GetBoardPosition();
        var positionIdentities = new HashSet<string>();
        foreach (var p in currentPositions)
        {
            if (p == null)
            {
                continue;
            }
            positionIdentities.Add(p.ToIdentity());
        }

        foreach (var position in boardPosition)
        {
            if (position == null)
            {
                continue;
            }
            if (positionIdentities.Contains(new int[] { position[0], position[1] }.ToIdentity()))
            {
                continue;
            }

            if (position[0] < 0 || position[1] < 0 || position[0] >= _boardRender.Width || position[1] >= _boardRender.Height)
            {
                return false;
            }

            if (_boardRender.Tiles[position[0], position[1]].HasTetromino())
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
}
