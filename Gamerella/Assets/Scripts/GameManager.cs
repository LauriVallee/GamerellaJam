using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Transform> pieces;
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;
    [SerializeField] Sprite[] sprites;

    private int emptyLocation;
    private int size;

    private void CreateGamePieces(float gapThickness)
    {
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

                piece.GetComponent<Tile>().index = count;

                piece.GetComponent<SpriteRenderer>().sprite = sprites[count];
                count++;

                if ((row == size - 1) && (col == size - 1))
                {
                    emptyLocation = count;
                    piece.gameObject.SetActive(false);
                }
            }   
        }
    
    }


    // Start is called before the first frame update
    void Start()
    {
        pieces = new List<Transform>();
        size = 3;
        CreateGamePieces(0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        // On click send out ray to see if we click a piece.
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
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

}
