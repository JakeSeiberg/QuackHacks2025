using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Room oneByOnePrefab;
    public Door doorPrefab;

    public static RoomManager instance;

    private void Awake() { instance = this; }

    public void SetupDoors(Room room, Cell cell)
    {
        TryDoor(cell.index - 10, EdgeDirection.Up, room, cell);
        TryDoor(cell.index + 10, EdgeDirection.Down, room, cell);
        TryDoor(cell.index - 1, EdgeDirection.Left, room, cell);
        TryDoor(cell.index + 1, EdgeDirection.Right, room, cell);
    }

    private void TryDoor(int neighborIndex, EdgeDirection dir, Room room, Cell originCell)
    {
        if (neighborIndex < 0 || neighborIndex >= 100) return;

        if (MapGenerator.instance.getFloorPlan[neighborIndex] != 1) return;

        Door door = Instantiate(doorPrefab, room.transform);
        door.transform.localPosition = GetDoorOffset(dir);
        door.SetDoorType(originCell.roomType, RoomManager.instance.GetNeighborCell(neighborIndex).roomType, dir);
    }

    public Cell GetNeighborCell(int index)
    {
        return MapGenerator.instance.getSpawnedCells.First(c => c.index == index);
    }

    private Vector2 GetDoorOffset(EdgeDirection dir)
    {
        switch(dir)
        {
            case EdgeDirection.Up: return new Vector2(0, 1.75f);
            case EdgeDirection.Down: return new Vector2(0, -1.75f);
            case EdgeDirection.Left: return new Vector2(-4.25f, 0);
            case EdgeDirection.Right: return new Vector2(4.25f, 0);
        }
        return Vector2.zero;
    }
}
