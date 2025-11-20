using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public Room oneByOnePrefab;
    public Room BossRoomPrefabs;
    public Room SpawnRoomPrefabs;
    public Door doorPrefab;
    public static RoomManager instance;
    
    private HashSet<string> createdDoors = new HashSet<string>();
    private List<Door> spawnedDoors = new List<Door>();

    private void Awake() { instance = this; }
    
    public void ResetDoors()
    {
        createdDoors.Clear();
        
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
        EdgeDirection3[] directions = new EdgeDirection3[]
        {
            EdgeDirection3.Up,
            EdgeDirection3.Down,
            EdgeDirection3.Left,
            EdgeDirection3.Right
        };
        
        for (int i = 0; i < 4; i++)
        {
            int nIndex = neighbors[i];
            if (nIndex < 0 || nIndex >= 100)
                continue;
            
            // Check if column is valid for left/right
            if (i == 2) // Left
            {
                int col = index % 10;
                if (col == 0) continue;
            }
            if (i == 3) // Right
            {
                int col = index % 10;
                if (col == 9) continue;
            }
            
            if (MapGenerator.instance.getFloorPlan[nIndex] == 1)
            {
                SpawnDoor(room, cell, nIndex, directions[i]);
            }
        }
    }
    
    private void SpawnDoor(Room room, Cell originCell, int neighborIndex, EdgeDirection3 dir)
    {
        if (neighborIndex < 0 || neighborIndex >= 100) return;

        if (MapGenerator.instance.getFloorPlan[neighborIndex] != 1) return;

        string doorID = GetDoorID(originCell.index, neighborIndex);
        
        if (createdDoors.Contains(doorID))
            return;
        
        createdDoors.Add(doorID);

        Vector3 doorWorldPosition = GetDoorWorldPosition(originCell, neighborIndex, dir);

        if (Random.Range(-10.0f, 10.0f) > 0f) {
            doorWorldPosition += new Vector3(245,0,0);
        }

        Door door = Instantiate(doorPrefab, doorWorldPosition, Quaternion.identity);
        
        spawnedDoors.Add(door);
        
        Debug.Log($"Spawned door at {doorWorldPosition} with direction: {dir} connecting cells {originCell.index} and {neighborIndex}");
        
        // Get the DoorTeleport component from the Door
        DoorTeleport doorTeleport = door.GetComponent<DoorTeleport>();
        if (doorTeleport != null)
        {
            // Convert EdgeDirection to EdgeDirection2
            EdgeDirection2 dir2 = (EdgeDirection2)((int)dir);
            doorTeleport.SetDoorType(originCell.roomType, RoomManager.instance.GetNeighborCell(neighborIndex).roomType, dir2);
        }
    }
        
    private string GetDoorID(int index1, int index2)
    {
        int smaller = Mathf.Min(index1, index2);
        int larger = Mathf.Max(index1, index2);
        return $"{smaller}-{larger}";
    }

    public Cell GetNeighborCell(int index)
    {
        return MapGenerator.instance.getSpawnedCells.First(c => c.index == index);
    }

    private Vector3 GetDoorWorldPosition(Cell originCell, int neighborIndex, EdgeDirection3 dir)
    {
        Cell neighborCell = GetNeighborCell(neighborIndex);
        
        Vector3 originPos = originCell.transform.position;
        Vector3 neighborPos = neighborCell.transform.position;
        
        Vector3 midpoint = (originPos + neighborPos) / 2f;
        
        float verticalOffset = 3.25f;
        float horizontalOffset = -6f;
        midpoint.y += verticalOffset;
        midpoint.x += horizontalOffset;
        
        return midpoint;
    }

    private Vector3 GetDoorWorldPositionAtEdge(Cell originCell, EdgeDirection3 dir)
    {
        Vector3 cellPos = originCell.transform.position;
        float halfWidth = MapGenerator.instance.roomWidth / 2f;
        float halfHeight = MapGenerator.instance.roomHeight / 2f;
        
        switch(dir)
        {
            case EdgeDirection3.Up: 
                return cellPos + new Vector3(0, halfHeight, 0);
            case EdgeDirection3.Down: 
                return cellPos + new Vector3(0, -halfHeight, 0);
            case EdgeDirection3.Left: 
                return cellPos + new Vector3(-halfWidth, 0, 0);
            case EdgeDirection3.Right: 
                return cellPos + new Vector3(halfWidth, 0, 0);
        }
        return cellPos;
    }

    private Vector2 GetDoorOffset(EdgeDirection3 dir)
    {
        float halfWidth = MapGenerator.instance.roomWidth / 2f;
        float halfHeight = MapGenerator.instance.roomHeight / 2f;
        
        switch (dir)
        {
            case EdgeDirection3.Up:    return new Vector3(0, halfHeight, 0);
            case EdgeDirection3.Down:  return new Vector3(0, -halfHeight, 0);
            case EdgeDirection3.Left:  return new Vector3(-halfWidth, 0, 0);
            case EdgeDirection3.Right: return new Vector3(halfWidth, 0, 0);
        }
        return Vector3.zero;
    }
}

// Keep the original EdgeDirection enum
public enum EdgeDirection3
{
    Up,
    Down,
    Left,
    Right
}