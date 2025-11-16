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
        int index = cell.index;
        int[] neighbors = new int[]
        {
            index - 10, // Up
            index + 10, // Down
            index - 1,  // Left
            index + 1   // Right
        };
        EdgeDirection[] directions = new EdgeDirection[]
        {
            EdgeDirection.Up,
            EdgeDirection.Down,
            EdgeDirection.Left,
            EdgeDirection.Right
        };
        
        for (int i = 0; i < 4; i++)
        {
            int nIndex = neighbors[i];
            if (nIndex < 0 || nIndex >= 100)
                continue;
            if (MapGenerator.instance.getFloorPlan[nIndex] == 1)
            {
                SpawnDoor(room, cell, nIndex, directions[i]);
            }
        }
    }
    
    private void SpawnDoor(Room room, Cell originCell, int neighborIndex, EdgeDirection dir)
    {
        Cell neighbor = MapGenerator.instance.getSpawnedCells.Find(c => c.index == neighborIndex);
        if (neighbor == null) 
            return;
        
        Door door = Instantiate(doorPrefab, room.transform);
        door.transform.localPosition = GetDoorOffset(dir);
        door.SetDoorType(originCell.roomType, neighbor.roomType, dir);
    }
    
    public Cell GetNeighborCell(int index)
    {
        return MapGenerator.instance.getSpawnedCells.First(c => c.index == index);
    }
    
    private Vector3 GetDoorOffset(EdgeDirection dir)
    {
        float halfWidth = MapGenerator.instance.roomWidth / 2f;
        float halfHeight = MapGenerator.instance.roomHeight / 2f;
        
        switch (dir)
        {
            case EdgeDirection.Up:    return new Vector3(0, halfHeight, 0);
            case EdgeDirection.Down:  return new Vector3(0, -halfHeight, 0);
            case EdgeDirection.Left:  return new Vector3(-halfWidth, 0, 0);
            case EdgeDirection.Right: return new Vector3(halfWidth, 0, 0);
        }
        return Vector3.zero;
    }
}