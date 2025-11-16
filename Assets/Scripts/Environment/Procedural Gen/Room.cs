using System.Collections.Generic;
using UnityEngine;

public enum EdgeDirection
{
    Up,
    Down,
    Left,
    Right
}

public class Room : MonoBehaviour
{
    public Cell.RoomType roomType;
    public int gridIndex; // Single grid index for 1x1 rooms
    
    // Call this when instantiated by RoomManager
    public void SetupRoom(Cell cell)
    {
        roomType = cell.roomType;
        gridIndex = cell.index;
    }
}