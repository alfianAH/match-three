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
                
                // Create candies
                GameObject candy = Instantiate(candies[index], pos, Quaternion.identity);
                
                candy.name = $"{i}, {j})";
                tiles[i, j] = candy;
            }
        }
    }
}
