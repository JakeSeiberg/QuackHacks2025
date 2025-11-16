using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private List<Room> createdRooms;
    public float xOff;
    public float yOff;

    public Room roomPrefab;
    public Door doorPrefab;

    public DoorScriptable[] doors;
    public RoomScriptable[] rooms;

    public static RoomManager instance;

    private void Awake()
    {
        instance = this;
        createdRooms = new List<Room>();
    }

    public void SetupRooms(List<Cell> spawnedCells)
    {
        for (int i = createdRooms.Count - 1; i >= 0; i--)
        {
            Destroy(createdRooms[i].gameObject);
        }
        createdRooms.Clear();

        foreach (var currentCell in spawnedCells)
        {
            var foundRoom = rooms.FirstOrDefault(x => x.roomShape == currentCell.roomShape && x.roomType == currentCell.roomType && doesTileMatch(x.occupiedTiles, currentCell));
            var currentPosition = currentCell.transform.position;
            var convertedPosition = new Vector2(currentPosition.x * xOff, currentPosition.y * yOff);
            var spawnedRoom = Instantiate(roomPrefab, convertedPosition, Quaternion.identity);

            spawnedRoom.SetupRoom(currentCell, foundRoom);
            createdRooms.Add(spawnedRoom);
        }
    }

    private bool doesTileMatch(int[] occupiedTiles, Cell cell)
    {
        if (occupiedTiles.Length != cell.cellList.Count){
            return false;
        }
        int minIndex = cell.cellList.Min();
        List<int> normalizedCell = new List<int>();
        foreach (int index in cell.cellList)
        {
            int dx = (index % 10) - (minIndex % 10);
            int dy = (index / 10) - (minIndex / 10);

            normalizedCell.Add(dy * 10 * dx);
        }
        normalizedCell.Sort();

        int[] sortedOccupied = (int[])occupiedTiles.Clone();
        Array.Sort(sortedOccupied);

        return normalizedCell.SequenceEqual(sortedOccupied);
    }
}
