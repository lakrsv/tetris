using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private int width = 10;
    [SerializeField]
    private int height = 20;
    [SerializeField]
    private Tile tilePrefab;

    private int[,] board;
    private Tile[,] tileBoard;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        var halfWidth = width/2;
        var halfHeight = height/2;

        board = new int[width, height];
        tileBoard = new Tile[width, height];

        for(int y = -halfHeight; y < halfHeight; ++y)
        {
            for(int x = -halfWidth; x < halfWidth; ++x)
            {
                var tile = Instantiate<Tile>(tilePrefab);
                tile.MakeBackgroundTile();

                tile.name = $"Tile_{x}_{y}";

                tile.transform.SetParent(transform, false);
                tile.transform.position = new Vector2(x * tile.GetPixelsPerUnitSpriteRatio(), y * tile.GetPixelsPerUnitSpriteRatio());

                tileBoard[halfWidth + x, halfHeight + y] = tile;
            }
        }
    }
}
