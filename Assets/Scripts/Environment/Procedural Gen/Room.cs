using System.Collections.Generic;
using UnityEngine;

public enum EdgeDirection
{
    Up,
    Down,
    Left,
    Right
};

public class Room : MonoBehaviour
{
    public Cell.RoomType roomType;

    public void SetupRoom(Cell cell)
    {
        roomType = cell.roomType;
    }
}
