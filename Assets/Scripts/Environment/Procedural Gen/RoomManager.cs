using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public Room oneByOnePrefab;
    public Room BossRoomPrefabs; 
    public Door doorPrefab;
    public static RoomManager instance;
    
    private HashSet<string> createdDoors = new HashSet<string>();
    private List<Door> spawnedDoors = new List<Door>(); // ADD THIS LINE

    private void Awake() { instance = this; }
    
    public void ResetDoors()
    {
        createdDoors.Clear();
        
        // ADD THESE LINES to destroy all doors
        foreach (var door in spawnedDoors)
        {
            if (door != null)
                Destroy(door.gameObject);
        }
        spawnedDoors.Clear();
    }

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
        if (neighborIndex < 0 || neighborIndex >= 100) return;

        if (MapGenerator.instance.getFloorPlan[neighborIndex] != 1) return;

        string doorID = GetDoorID(originCell.index, neighborIndex);
        
        if (createdDoors.Contains(doorID))
            return;
        
        createdDoors.Add(doorID);

        // USE MIDPOINT instead of edge position
        Vector3 doorWorldPosition = GetDoorWorldPosition(originCell, neighborIndex, dir);

        Door door = Instantiate(doorPrefab, doorWorldPosition, Quaternion.identity);
        
        spawnedDoors.Add(door);
        
        door.SetDoorType(originCell.roomType, RoomManager.instance.GetNeighborCell(neighborIndex).roomType, dir);
    }
        
    private string GetDoorID(int index1, int index2)
    {
        // Always use the smaller index first to ensure the same door gets the same ID
        // regardless of which room creates it
        int smaller = Mathf.Min(index1, index2);
        int larger = Mathf.Max(index1, index2);
        return $"{smaller}-{larger}";
    }

    public Cell GetNeighborCell(int index)
    {
        return MapGenerator.instance.getSpawnedCells.First(c => c.index == index);
    }

    private Vector3 GetDoorWorldPosition(Cell originCell, int neighborIndex, EdgeDirection dir)
    {
        Cell neighborCell = GetNeighborCell(neighborIndex);
        
        // Get positions of both rooms
        Vector3 originPos = originCell.transform.position;
        Vector3 neighborPos = neighborCell.transform.position;
        
        // Door should be exactly between the two rooms (midpoint)
        Vector3 midpoint = (originPos + neighborPos) / 2f;
        
        // Add vertical offset to move doors down
        float verticalOffset = 3.25f; // Adjust this value as needed (negative = down, positive = up)
        float horizontalOffset = -6f;
        midpoint.y += verticalOffset;
        midpoint.x += horizontalOffset;
        
        return midpoint;
    }

    // Alternative: Position door at edge of current room
    private Vector3 GetDoorWorldPositionAtEdge(Cell originCell, EdgeDirection dir)
    {
        Vector3 cellPos = originCell.transform.position;
        float halfWidth = MapGenerator.instance.roomWidth / 2f;
        float halfHeight = MapGenerator.instance.roomHeight / 2f;
        
        switch(dir)
        {
            case EdgeDirection.Up: 
                return cellPos + new Vector3(0, halfHeight, 0);
            case EdgeDirection.Down: 
                return cellPos + new Vector3(0, -halfHeight, 0);
            case EdgeDirection.Left: 
                return cellPos + new Vector3(-halfWidth, 0, 0);
            case EdgeDirection.Right: 
                return cellPos + new Vector3(halfWidth, 0, 0);
        }
        return cellPos;
    }

    // Old method - no longer needed if using world positions
    private Vector2 GetDoorOffset(EdgeDirection dir)
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