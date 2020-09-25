using UnityEngine;

public class Tile : MonoBehaviour
{
    public float xPosition,
        yPosition;
    public int column, 
        row;

    private Grid grid;
    private GameObject otherTile;
    private Vector3 firstPosition,
        finalPosition,
        tempPosition;
    private float swipeAngle;

    private void Start()
    {
        // Set position from tile
        grid = FindObjectOfType<Grid>();
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        column = Mathf.RoundToInt((xPosition - grid.startPos.x) / grid.offset.x);
        row = Mathf.RoundToInt((yPosition - grid.startPos.y) / grid.offset.y);
    }

    private void Update()
    {
        xPosition = column * grid.offset.x + grid.startPos.x;
        yPosition = row * grid.offset.y + grid.startPos.y;
        SwipeTile();
    }

    private void OnMouseDown()
    {
        // Get first point
        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        // Get final point
        finalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }
    
    /// <summary>
    /// Calculate angle
    /// </summary>
    private void CalculateAngle()
    {
        // Calculate angle between first position and final position
        swipeAngle = Mathf.Atan2(finalPosition.y - firstPosition.y, finalPosition.x - firstPosition.x) * 180 / Mathf.PI;
        
        MoveTile();
    }
    
    /// <summary>
    /// Detect swipe angle
    /// </summary>
    private void MoveTile()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < grid.gridSizeX)
        {
            // Right swipe
            Debug.Log("Right swipe");
            SwipeRightMove();
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < grid.gridSizeY)
        {
            // Up swipe
            Debug.Log("Up swipe");
            SwipeUpMove();
        } 
        else if (swipeAngle > 135 || swipeAngle <= -135 && column > 0 )
        {
            // Left swipe
            Debug.Log("Left swipe");
            SwipeLeftMove();
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // Down swipe
            Debug.Log("Down swipe");
            SwipeDownMove();
        }
    }

    private void SwipeTile()
    {
        if (Mathf.Abs(xPosition - transform.position.x) > 0.1f)
        {
            // Move towards the target
            tempPosition = new Vector3(xPosition, transform.position.y);
            transform.position = Vector3.Lerp(transform.position, tempPosition, 0.4f);
        }
        else
        {
            // Directly set the position
            tempPosition = new Vector3(xPosition, transform.position.y);
            transform.position = tempPosition;
            grid.tiles[column, row] = gameObject;
        }

        if (Mathf.Abs(yPosition - transform.position.y) > 0.1f)
        {
            // Move toward the target
            tempPosition = new Vector3(transform.position.x, yPosition);
            transform.position = Vector3.Lerp(transform.position, tempPosition, 0.4f);
        }
        else
        {
            // Directly set the position
            tempPosition = new Vector3(transform.position.x, yPosition);
            transform.position = tempPosition;
            grid.tiles[column, row] = gameObject;
        }
    }

    private void SwipeRightMove()
    {
        if (column+1 < grid.gridSizeX)
        {
            // Swipe tile position to the right
            otherTile = grid.tiles[column + 1, row];
            otherTile.GetComponent<Tile>().column -= 1;
            column += 1;
        }
    }
    
    private void SwipeUpMove()
    {
        if (row+1 < grid.gridSizeY)
        {
            // Swipe tile position to the top
            otherTile = grid.tiles[column, row + 1];
            otherTile.GetComponent<Tile>().row -= 1;
            row += 1;
        }
    }
    
    private void SwipeLeftMove()
    {
        if (column-1 >= 0)
        {
            // Swipe tile position to the left
            otherTile = grid.tiles[column - 1, row];
            otherTile.GetComponent<Tile>().column += 1;
            column -= 1;
        }
    }
    
    private void SwipeDownMove()
    {
        if (row-1 >= 0)
        {
            // Swipe tile position to the bottom
            otherTile = grid.tiles[column, row - 1];
            otherTile.GetComponent<Tile>().row += 1;
            row -= 1;
        }
    }
}
