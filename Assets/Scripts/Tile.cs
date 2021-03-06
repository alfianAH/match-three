﻿using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isMatched;
    public float xPosition,
        yPosition;
    public int column, 
        row;

    private Grid grid;
    private GameObject otherTile;
    private SpriteRenderer tileSprite;
    private Vector3 firstPosition,
        finalPosition,
        tempPosition;
    private float swipeAngle;
    private int previousColumn,
        previousRow;

    private void Start()
    {
        tileSprite = GetComponent<SpriteRenderer>();
        // Set position from tile
        grid = FindObjectOfType<Grid>();
        
        var position = transform.position;
        xPosition = position.x;
        yPosition = position.y;
        
        column = Mathf.RoundToInt((xPosition - grid.startPos.x) / grid.offset.x);
        row = Mathf.RoundToInt((yPosition - grid.startPos.y) / grid.offset.y);
    }

    private void Update()
    {
        CheckMatches();
        xPosition = column * grid.offset.x + grid.startPos.x;
        yPosition = row * grid.offset.y + grid.startPos.y;
        SwipeTile();

        if (isMatched)
        {
            tileSprite.color = Color.grey;
        }
    }

    private void OnMouseDown()
    {
        // Get first point
        if (Camera.main != null) 
            firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        // Get final point
        if (Camera.main != null) 
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
        previousRow = row;
        previousColumn = column;
        
        if (swipeAngle > -45 && swipeAngle <= 45)
        {
            // Right swipe
            // Debug.Log("Right swipe");
            SwipeRightMove();
        }
        else if (swipeAngle > 45 && swipeAngle <= 135)
        {
            // Up swipe
            // Debug.Log("Up swipe");
            SwipeUpMove();
        } 
        else if (swipeAngle > 135 || swipeAngle <= -135)
        {
            // Left swipe
            // Debug.Log("Left swipe");
            SwipeLeftMove();
        }
        else if (swipeAngle < -45 && swipeAngle >= -135)
        {
            // Down swipe
            // Debug.Log("Down swipe");
            SwipeDownMove();
        }

        StartCoroutine(CheckMove());
    }
    
    /// <summary>
    /// Check tile matching
    /// </summary>
    private void CheckMatches()
    {
        // Check horizontal matching
        if (column > 0 && column < grid.gridSizeX - 1)
        {
            // Check the other side
            GameObject leftTile = grid.tiles[column - 1, row];
            GameObject rightTile = grid.tiles[column + 1, row];

            if (leftTile != null && rightTile != null)
            {
                if (leftTile.CompareTag(gameObject.tag) && rightTile.CompareTag(gameObject.tag))
                {
                    isMatched = true;
                    rightTile.GetComponent<Tile>().isMatched = true;
                    leftTile.GetComponent<Tile>().isMatched = true;
                }
            }
        }
        
        // Check vertical matching
        if (row > 0 && row < grid.gridSizeY - 1)
        {
            // Check up and down
            GameObject upTile = grid.tiles[column, row + 1];
            GameObject downTile = grid.tiles[column, row - 1];

            if (upTile != null && downTile != null)
            {
                if (upTile.CompareTag(gameObject.tag) && downTile.CompareTag(gameObject.tag))
                {
                    isMatched = true;
                    downTile.GetComponent<Tile>().isMatched = true;
                    upTile.GetComponent<Tile>().isMatched = true;
                }
            }
        }
    }
    
    /// <summary>
    /// Check move. If there are same tiles, destroy it
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(0.5f);
        
        // If there is same tile, DestroyMatches
        if (otherTile != null)
        {
            Tile otherTileComponent = otherTile.GetComponent<Tile>();
            
            if (!isMatched && !otherTileComponent.isMatched)
            {
                otherTileComponent.row = row;
                otherTileComponent.column = column;
                row = previousRow;
                column = previousColumn;
                GameManager.instance.Combo = 0;
            }
            else
            {
                grid.DestroyMatches();
            }
        }

        otherTile = null;
    }

    private void SwipeTile()
    {
        if (Mathf.Abs(xPosition - transform.position.x) > 0.1f)
        {
            // Move towards the target
            var position = transform.position;
            tempPosition = new Vector3(xPosition, position.y);
            position = Vector3.Lerp(position, tempPosition, 0.4f);
            transform.position = position;
        }
        else
        {
            // Directly set the position
            var currentTransform = transform;
            tempPosition = new Vector3(xPosition, currentTransform.position.y);
            currentTransform.position = tempPosition;
            grid.tiles[column, row] = gameObject;
        }

        if (Mathf.Abs(yPosition - transform.position.y) > 0.1f)
        {
            // Move towards the target
            var position = transform.position;
            tempPosition = new Vector3(position.x, yPosition);
            position = Vector3.Lerp(position, tempPosition, 0.4f);
            transform.position = position;
        }
        else
        {
            // Directly set the position
            var currentTransform = transform;
            tempPosition = new Vector3(currentTransform.position.x, yPosition);
            currentTransform.position = tempPosition;
            grid.tiles[column, row] = gameObject;
        }
    }
    
    /// <summary>
    /// Swipe right
    /// </summary>
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
    
    /// <summary>
    /// Swipe up
    /// </summary>
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
    
    /// <summary>
    /// Swipe left
    /// </summary>
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
    
    /// <summary>
    /// Swipe down
    /// </summary>
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
