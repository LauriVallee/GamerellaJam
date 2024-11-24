using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int index;
    public Vector2Int coords;
    public Vector2Int winCoords;
    public GameManager gameManager;

    public void OnMouseDown()
    {
        gameManager.trySwap(this, true);
    }
}
