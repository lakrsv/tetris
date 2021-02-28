using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRender : MonoBehaviour
{
    [SerializeField]
    private int _width = 10;
    [SerializeField]
    private int _height = 20;
    [SerializeField]
    private Tile _tilePrefab;

    public Tile[,] Tiles { get; private set; }

    public int Width => _width;
    public int Height => _height;

    public void Render()
    {
        // Render all Tiles based on their type
        for (int x = 0; x < _width; ++x)
        {
            for (int y = 0; y < _height; ++y)
            {
                Tiles[x, y].Render(Sprites.Instance);
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        var halfWidth = _width / 2;
        var halfHeight = _height / 2;

        Tiles = new Tile[_width, _height];

        for (int y = -halfHeight; y < halfHeight; ++y)
        {
            for (int x = -halfWidth; x < halfWidth; ++x)
            {
                var tile = Instantiate(_tilePrefab);

                tile.name = $"Tile_{x}_{y}";

                tile.transform.SetParent(transform, false);
                tile.transform.position = new Vector2(x * tile.GetPixelsPerUnitSpriteRatio(), y * tile.GetPixelsPerUnitSpriteRatio());

                Tiles[halfWidth + x, halfHeight + y] = tile;
            }
        }
    }
}
