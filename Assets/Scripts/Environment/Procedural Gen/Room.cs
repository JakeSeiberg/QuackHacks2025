using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public SpriteRenderer spriteRenderer; 

    public void SetupRoom(Cell currentCell, RoomScriptable room)
    {
        spriteRenderer.sprite = room.roomVariations[Random.Range(0, room.roomVariations.Length)];

        var floorplan = MapGenerator.instance.getFloorPlan;
        var cellList = MapGenerator.instance.getSpawnedCells;

        switch (currentCell.roomShape)
        {
            case Cell.RoomShape.OnebyOne:
                SetupOnebyOne(currentCell, floorplan, cellList);
                break;
            case Cell.RoomShape.OnebyTwo:
                SetupOnebyTwo(currentCell, floorplan, cellList);
                break;
            case Cell.RoomShape.TwobyOne:
                SetupTwobyOne(currentCell, floorplan, cellList);
                break;
            case Cell.RoomShape.TwobyTwo:
                SetupTwobyTwo(currentCell, floorplan, cellList);
                break;
            default:
                break;
        }
    }
    public void SetupOnebyOne(Cell cell, int[] floorplan, List<Cell> cellList)
    { 
        var currentCell = cell.cellList[0];

        TryDoor(currentCell, new Vector2(0, 1.75f), EdgeDirection.Up, floorplan, cellList, cell);
        TryDoor(currentCell, new Vector2(0, -1.75f), EdgeDirection.Down, floorplan, cellList, cell);
        TryDoor(currentCell, new Vector2(-4.25f, 0), EdgeDirection.Left, floorplan, cellList, cell);
        TryDoor(currentCell, new Vector2(4.25f, 0), EdgeDirection.Right, floorplan, cellList, cell);
    }
    public void SetupOnebyTwo(Cell cell, int[] floorplan, List<Cell> cellList)
    { 
        var cellA = cell.cellList[0];
        var cellB = cell.cellList[1];

        TryDoor(cellA, new Vector2(0, 4f), EdgeDirection.Up, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(-4.25f, 2.615f), EdgeDirection.Left, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(-4.25f, 2.615f), EdgeDirection.Right, floorplan, cellList, cell);

        TryDoor(cellB, new Vector2(0, -4f), EdgeDirection.Down, floorplan, cellList, cell);
        TryDoor(cellB, new Vector2(-4.25f, -2.615f), EdgeDirection.Left, floorplan, cellList, cell);
        TryDoor(cellB, new Vector2(4.25f, -2.615f), EdgeDirection.Right, floorplan, cellList, cell);
    }
    public void SetupTwobyOne(Cell cell, int[] floorplan, List<Cell> cellList)
    { 
        var cellA = cell.cellList[0];
        var cellB = cell.cellList[1];

        TryDoor(cellA, new Vector2(-5f, 1.5f), EdgeDirection.Up, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(-9.75f, 0f), EdgeDirection.Left, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(-5f, -1.5f), EdgeDirection.Down, floorplan, cellList, cell);

        TryDoor(cellA, new Vector2(5f, 1.5f), EdgeDirection.Up, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(5f, -1.5f), EdgeDirection.Down, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(9.75f, 0f), EdgeDirection.Right, floorplan, cellList, cell);
    }
    public void SetupTwobyTwo(Cell cell, int[] floorplan, List<Cell> cellList)
    { 
        var cellA = cell.cellList[0];
        var cellB = cell.cellList[1];
        var cellC = cell.cellList[2];
        var cellD = cell.cellList[3];

        TryDoor(cellA, new Vector2(-5.3125f, 4.5f), EdgeDirection.Up, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(5.3125f, 4.5f), EdgeDirection.Up,  floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(-9.75f, 2.6125f), EdgeDirection.Left, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(-9.75f, -2.6125f), EdgeDirection.Left, floorplan, cellList, cell);

        TryDoor(cellA, new Vector2(-5.3125f, -4.5f), EdgeDirection.Down, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(5.3125f, -4.5f), EdgeDirection.Down, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(9.75f, 2.6125f), EdgeDirection.Right, floorplan, cellList, cell);
        TryDoor(cellA, new Vector2(9.75f, -2.6125f), EdgeDirection.Right, floorplan, cellList, cell);
    }

    private void TryDoor(int fromIndex, Vector2 positionOffset, EdgeDirection direction, int[] floorplan, List<Cell> cellList, Cell currentCell)
    {
        int neighborIndex = fromIndex + GetOffset(direction);
        if (neighborIndex < 0 || neighborIndex >= floorplan.Length)
            return;
        if (floorplan[neighborIndex] != 1)
            return;
        var foundCell = cellList.FirstOrDefault(x => x.cellList.Contains(neighborIndex));
        var door = Instantiate(RoomManager.instance.doorPrefab, transform);
        door.transform.position = (Vector2)transform.position + positionOffset;
        SetupDoor(door, direction, currentCell.roomType == Cell.RoomType.Regular ? foundCell.roomType : currentCell.roomType);
    }

    private void SetupDoor(Door door, EdgeDirection direction, Cell.RoomType roomType)
    {
        var doorTypes = GetDoorOpts(roomType);

        switch (direction){
            case EdgeDirection.Up:
                door.setDoorSprite(doorTypes.upDoor);
                break;
            case EdgeDirection.Down:
                door.setDoorSprite(doorTypes.downDoor);
                break;
            case EdgeDirection.Left:
                door.setDoorSprite(doorTypes.leftDoor);
                break;
            case EdgeDirection.Right:
                door.setDoorSprite(doorTypes.rightDoor);
                break;
            default:
                break;
        }
    }
    private DoorScriptable GetDoorOpts(Cell.RoomType roomType)
    {
        return RoomManager.instance.doors.FirstOrDefault(x => x.roomType == roomType);
    }
    private int GetOffset(EdgeDirection direction)
    {
        switch (direction){
            case EdgeDirection.Up:
                return -10;
            case EdgeDirection.Down:
                return 10;
            case EdgeDirection.Left:
                return -1;
            case EdgeDirection.Right:
                return 1;
        }
        return 0;
    }
}
