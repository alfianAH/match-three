using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector3 firstPosition,
        finalPosition;
    private float swipeAngle;
    
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
        if (swipeAngle > -45 && swipeAngle <= 45)
        {
            // Right swipe
            Debug.Log("Right swipe");
        }
        else if (swipeAngle > 45 && swipeAngle <= 135)
        {
            // Up swipe
            Debug.Log("Up swipe");
        } 
        else if (swipeAngle > 135 || swipeAngle <= -135)
        {
            // Left swipe
            Debug.Log("Left swipe");
        }
        else if (swipeAngle < -45 && swipeAngle >= -135)
        {
            // Down swipe
            Debug.Log("Down swipe");
        }
    }
}
