using System.Collections;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int gridSizeX, gridSizeY; // Grid size
    public Vector3 startPos, offset;
    public GameObject tilePrefab;
    public GameObject[] candies;
    public GameObject[,] tiles;
    
    private void Start()
    {
        CreateGrid();
    }
    
    /// <summary>
    /// Create grid when game starts
    /// </summary>
    private void CreateGrid()
    {
        tiles = new GameObject[gridSizeX, gridSizeY];
        
        // Set offset from prefab's size
        offset = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;
        
        // Set start position
        startPos = transform.position + 
                   Vector3.left * (offset.x * gridSizeX / 2) + 
                   Vector3.down * (offset.y * gridSizeY / 3);
        
        // Make tiles
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 pos = new Vector3(startPos.x + i*offset.x, startPos.y + j*offset.y);
                GameObject backgroundTile = Instantiate(tilePrefab, pos, tilePrefab.transform.rotation);

                backgroundTile.transform.parent = transform;
                backgroundTile.name = $"({i}, {j})";
                
                int index = Random.Range(0, candies.Length);

                int maxIteration = 0;
                while (MatchesAt(i, j, candies[index]) && maxIteration < 100)
                {
                    index = Random.Range(0, candies.Length);
                    maxIteration++;
                }
                
                // Create candies
                GameObject candy = ObjectPooler.instance.SpawnFromPool(index.ToString(), pos, Quaternion.identity);
                
                candy.name = $"{i}, {j})";
                tiles[i, j] = candy;
            }
        }
    }
    
    /// <summary>
    /// Prevent matching tiles
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <param name="piece"></param>
    /// <returns></returns>
    private bool MatchesAt(int column, int row, GameObject piece)
    {
        // Check if there is the same tile under and next to it
        if (column > 1 && row > 1)
        {
            if (tiles[column - 1, row].CompareTag(piece.tag) && 
                tiles[column - 2, row].CompareTag(piece.tag))
            {
                return true;
            }

            if (tiles[column, row - 1].CompareTag(piece.tag) &&
                tiles[column, row - 2].CompareTag(piece.tag))
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            // Check if there is the same tile on top and nex to it
            if (row > 1)
            {
                if (tiles[column, row - 1].CompareTag(piece.tag) &&
                    tiles[column, row - 2].CompareTag(piece.tag))
                {
                    return true;
                }
            }

            if (column > 1)
            {
                if (tiles[column - 1, row].CompareTag(piece.tag) &&
                    tiles[column - 2, row].CompareTag(piece.tag))
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    private void DestroyMatchesAt(int column, int row)
    {
        // Destroy tile in certain index
        if (tiles[column, row].GetComponent<Tile>().isMatched)
        {
            GameManager.instance.AddScore(10);
            GameObject gm = tiles[column, row];
            gm.SetActive(false);
            tiles[column, row] = null;
        }
    }
    
    public void DestroyMatches()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if (tiles[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        
        StartCoroutine(DecreaseRow());
    }
    
    /// <summary>
    /// If there are null tiles, instance random candies 
    /// </summary>
    private void RefillBoard()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if (tiles[i, j] == null){
                    Vector3 tempPosition = new Vector3(startPos.x + i*offset.x, startPos.y + j*offset.y);
                    int candyToUse = Random.Range(0, candies.Length);
                    GameObject tileToRefill = ObjectPooler.instance.SpawnFromPool(candyToUse.ToString(), tempPosition, Quaternion.identity);
                    
                    tiles[i, j] = tileToRefill;
                }
            }
        }
    }
    
    /// <summary>
    /// To check every grids
    /// </summary>
    /// <returns></returns>
    private bool MatchesOnBoard()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if (tiles[i, j] != null)
                {
                    if (tiles[i, j].GetComponent<Tile>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    
    /// <summary>
    /// To decrease row which tile is null
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreaseRow()
    {
        int nullCount = 0;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if (tiles[i, j] == null)
                    nullCount++;
                else if (nullCount > 0)
                {
                    tiles[i, j].GetComponent<Tile>().row -= nullCount;
                    tiles[i, j] = null;
                }
            }

            nullCount = 0;
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(FillBoard());
    }
    
    /// <summary>
    /// Fill tile in board
    /// </summary>
    /// <returns></returns>
    private IEnumerator FillBoard()
    {
        RefillBoard();
        yield return new WaitForSeconds(0.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(0.5f);
            DestroyMatches();
        }
    }
}
