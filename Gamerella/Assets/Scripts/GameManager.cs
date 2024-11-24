using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Transform> pieces;
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;
    [SerializeField] Sprite[] sprites;


    private List<Tile> tiles = new List<Tile>();

    private Vector2Int emptyCoords;
    private Vector3    emptyPosition;
    private int emptyLocation;
    private int size;

    private void CreateGamePieces(float gapThickness)
    {
        Random.InitState(1337);

        float width = 1 / (float)size;

        int count = 0;

        for (int row = 0; row < size; row++) {
           for (int col = 0; col < size; col++) {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);
                piece.transform.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                                            +1 - (2 * width * row) - width,
                                                            0);
                piece.localScale = 0.23f * Vector3.one;
                piece.name = $"{(row * size) + col}";

                Tile tile = piece.GetComponent<Tile>();
                tile.index = count;
                tile.coords = new Vector2Int(col, row);
                tile.winCoords = tile.coords;
                tile.gameManager = this;
                piece.GetComponent<SpriteRenderer>().sprite = sprites[count];
                count++;

                if ((row == size - 1) && (col == size - 1))
                {
                    emptyCoords   = new Vector2Int(col, row);
                    emptyLocation = count;
                    emptyPosition = piece.transform.localPosition;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    tiles.Add(tile);
                }
            }   
        }

        shufflePuzzle(50);
    }


    // Start is called before the first frame update
    void Start()
    {
        pieces = new List<Transform>();
        size = 3;
        CreateGamePieces(0.01f);
    }

    // Update is called once per frame
    /*
    void Update()
    {
        // On click send out ray to see if we click a piece.
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                int index = hit.transform.GetComponent<Tile>().index;

                Debug.Log(index);

                // Check each direction to see if valid move.
                // We break out on success so we don't carry on and swap back again.
                if (SwapIfValid(index, -size, size)) { return; }
                if (SwapIfValid(index, +size, size)) { return; }
                if (SwapIfValid(index, -1, 0)) { return; }
                if (SwapIfValid(index, +1, size - 1)) { return; }
            }
        }
    }*/

    public void trySwap(Tile tile, bool checkWonPuzzle)
    {
        int index = tile.index;
        Vector2Int tileCoords = tile.coords;

        if(Mathf.Abs(tileCoords.x - emptyCoords.x) + Mathf.Abs(tileCoords.y - emptyCoords.y) == 1)
        {
            Vector3 oldTilePos = tile.transform.localPosition;
            tile.transform.localPosition = emptyPosition;
            emptyPosition = oldTilePos;

            Vector2Int oldTileCoords = tile.coords;
            tile.coords = emptyCoords;
            emptyCoords = oldTileCoords;
        }

        if(checkWonPuzzle && wonPuzzle())
        {
            Debug.Log("WOOOO WE WON!!!");
        }
    }

    public bool wonPuzzle()
    {
        foreach(Tile tile in tiles)
        {
            if(tile.coords != tile.winCoords)
            {
                return false;
            }
        }
        return true;
    }

    // colCheck is used to stop horizontal moves wrapping.
    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            // Swap them in game state.
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            // Swap their transforms.
            (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
            // Update empty location.
            emptyLocation = i;
            return true;
        }
        return false;
    }

    public void shufflePuzzle(int moveCount)
    {
        for(int i = 0; i < moveCount; ++i)
        {
            Vector2Int neighborCoords = getRandomNeighbor(emptyCoords);

            trySwap(getTileAtCoords(neighborCoords), false);
        }
    }

    public Vector2Int getRandomNeighbor(Vector2Int coords)
    {
        List<Vector2Int> validCoords = new List<Vector2Int>();

        Vector2Int leftCoord  = new Vector2Int(coords.x - 1, coords.y);
        Vector2Int rightCoord = new Vector2Int(coords.x + 1, coords.y);
        Vector2Int topCoord   = new Vector2Int(coords.x, coords.y + 1);
        Vector2Int botCoord   = new Vector2Int(coords.x, coords.y - 1);

        if (isValidCoord(leftCoord))
        {
            validCoords.Add(leftCoord);
        }
        if (isValidCoord(rightCoord))
        {
            validCoords.Add(rightCoord);
        }
        if (isValidCoord(topCoord))
        {
            validCoords.Add(topCoord);
        }
        if (isValidCoord(botCoord))
        {
            validCoords.Add(botCoord);
        }

        return validCoords[Random.Range(0, validCoords.Count)];
    }

    public Tile getTileAtCoords(Vector2Int coords)
    {
        foreach(Tile tile in tiles)
        {
            if(tile.coords == coords)
            {
                return tile;
            }
        }
        return null;
    }

    public bool isValidCoord(Vector2Int coords)
    {
        return coords.x >= 0 && coords.x < size
            && coords.y >= 0 && coords.y < size;
    }
}
