using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using TMPro;

public class MapGenerator : MonoBehaviour
{  
    private int[] floorPlan;
    public int[] getFloorPlan => floorPlan;

    private int floorPlanCount;
    private int minRooms;
    private int maxRooms;
    private List<int> endRooms;

    public float roomWidth = 19f;   
    public float roomHeight = 15f;  

    private int BossRoomIndex;
    private int shopRoomIndex;
    private int itemRoomIndex;

    public Cell cellPrefab;
    private float cellSize;
    private Queue<int> cellQueue;
    private List<Cell> spawnedCells;
    private Dictionary<int, Cell> indexToCellMap;
    public List<Cell> getSpawnedCells => spawnedCells;

    [SerializeField] private Sprite item;
    [SerializeField] private Sprite shop;
    [SerializeField] private Sprite boss;

    [SerializeField] private GameObject[] oneByOneRoomPrefabs; // Array of 1x1 room variants

    public static MapGenerator instance;

    void Start()
    {
        instance = this;
        minRooms = 5;
        maxRooms = 7;
        cellSize = 20f;
        spawnedCells = new();

        SetupDungeon();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetupDungeon();
        }
    }

    void SetupDungeon()
    {
        RoomManager.instance.ResetDoors();

        for (int i = 0; i < spawnedCells.Count; i++)
        {
            Destroy(spawnedCells[i].gameObject);
        }

        spawnedCells.Clear();

        floorPlan = new int[100];
        floorPlanCount = default;
        cellQueue = new Queue<int>();
        endRooms = new List<int>();
        indexToCellMap = new Dictionary<int, Cell>();

        VisitCell(45);
        GenerateDungeon();
    }
    
    void GenerateDungeon()
    {
        while (cellQueue.Count > 0)
        {
            int index = cellQueue.Dequeue();
            int x = index % 10;
            bool created = false;

            if (x > 1) created |= VisitCell(index - 1);
            if (x < 9) created |= VisitCell(index + 1);
            if (index > 20) created |= VisitCell(index - 10);
            if (index < 70) created |= VisitCell(index + 10);

            if (created == false)
                endRooms.Add(index);
        }
        
        if (floorPlanCount < minRooms)
        {
            SetupDungeon();
            return;
        }
        
        CleanEndRoomList();
        SetupSpecialRooms();
    }

    void CleanEndRoomList()
    {
        endRooms.RemoveAll(item => GetNeighborCount(item) > 1);
    }
    
    void SetupSpecialRooms()
    {
        BossRoomIndex = endRooms.Count > 0 ? endRooms[endRooms.Count - 1 ] : -1;
        if (BossRoomIndex != -1)
            endRooms.RemoveAt(endRooms.Count - 1);

        itemRoomIndex = RandomEndRoom();
        shopRoomIndex = RandomEndRoom();

        if (itemRoomIndex == -1 || shopRoomIndex == -1 || BossRoomIndex == -1)
        {
            SetupDungeon();
            return;
        }

        // Change cell visuals
        ChangeSpecialVisuals();

        // Create a map to store spawned rooms
        Dictionary<int, Room> roomMap = new Dictionary<int, Room>();
        
        // Spawn all rooms
        foreach (var cell in spawnedCells)
        {
            GameObject roomGO = Instantiate(
                RoomManager.instance.oneByOnePrefab.gameObject,
                cell.transform.position,  // Use cell's transform position
                Quaternion.identity
            );

            Room roomComp = roomGO.GetComponent<Room>();
            roomComp.SetupRoom(cell);
            roomMap[cell.index] = roomComp;
        }

        // Setup doors for all rooms
        foreach (var cell in spawnedCells)
        {
            Room roomComp = roomMap[cell.index];
            RoomManager.instance.SetupDoors(roomComp, cell);
        }
        
        // Hide or destroy the cell visual objects (keep the Cell component for reference)
        foreach (var cell in spawnedCells)
        {
            // Option 1: Hide the cell's sprite renderer
            if (cell.spriteRenderer != null)
                cell.spriteRenderer.enabled = false;
            
            // Option 2: Or completely disable the cell's GameObject visuals
            // cell.gameObject.SetActive(false);
        }
    }

    
    void ChangeSpecialVisuals()
    {
        foreach (var cell in spawnedCells)
        {
            if(cell.index == itemRoomIndex)
            {
                cell.setSpecialRoomSprite(item);
                cell.setRoomType(Cell.RoomType.Item);
            }
            if(cell.index == shopRoomIndex)
            {
                cell.setSpecialRoomSprite(shop);
                cell.setRoomType(Cell.RoomType.Shop);
            }
            if(cell.index == BossRoomIndex)
            {
                cell.setSpecialRoomSprite(boss);
                cell.setRoomType(Cell.RoomType.Boss);
            }
        }
    }
    
    int RandomEndRoom()
    {
        if (endRooms.Count == 0)
        {
            return -1;
        }
        int RandomRoom = Random.Range(0, endRooms.Count);
        int index = endRooms[RandomRoom];

        endRooms.RemoveAt(RandomRoom);

        return index;
    }
    
    private int GetNeighborCount(int index)
    {
        int count = 0;

        if (floorPlan[index - 10] == 1)
            count++;
        if (floorPlan[index - 1] == 1)
            count++;
        if (floorPlan[index + 1] == 1)
            count++;
        if (floorPlan[index + 10] == 1)
            count++;
            
        return count;
    }
    
    private bool VisitCell(int index)
    {
        if (floorPlan[index] != 0)
            return false;
        if (GetNeighborCount(index) > 1)
            return false;
        if (floorPlanCount > maxRooms)
            return false;
        if (Random.value < 0.5f)
            return false;

        cellQueue.Enqueue(index);
        floorPlan[index] = 1;
        floorPlanCount++;

        SpawnRoom(index);

        return true;
    }
    
    private void SpawnRoom(int index)
    {
        int x = index % 10;
        int y = index / 10;

        // Center around (0, 0) with perfect alignment
        Vector2 position = new Vector2(
            (x - 4.5f) * roomWidth,
            -(y - 4.5f) * roomHeight
        );

        Cell newCell = Instantiate(cellPrefab, position, Quaternion.identity);

        newCell.value = 1;
        newCell.index = index;
        newCell.setRoomType(Cell.RoomType.Regular);
        newCell.worldPosition = position;

        spawnedCells.Add(newCell);
        indexToCellMap[index] = newCell;
    }
}