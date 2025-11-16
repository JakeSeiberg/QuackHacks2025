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

    private int BossRoomIndex;
    private int shopRoomIndex;
    private int itemRoomIndex;

    public Cell cellPrefab;
    private float cellSize;
    private Queue<int> cellQueue;
    private List<Cell> spawnedCells;
    private List<int> bigRoomIndexes;
    private Dictionary<int, Cell> indexToCellMap;
    public List<Cell> getSpawnedCells => spawnedCells;

    [SerializeField] private Sprite item;
    [SerializeField] private Sprite shop;
    [SerializeField] private Sprite boss;

    [SerializeField] private Sprite largeRoom;
    [SerializeField] private Sprite verticalRoom;
    [SerializeField] private Sprite horizontalRoom;

    public static MapGenerator instance;

    private static readonly List<int[]> roomShapes = new()
    {
        new int[]{10 },
        new int[]{-10 },
        
        new int[]{-1 },
        new int[]{1 },
        
        new int[] {1,10,11 }
    };

    void Start()
    {
        instance = this;
        minRooms = 5;
        maxRooms = 7;
        cellSize = 0.5f;
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
        for (int i = 0; i < spawnedCells.Count; i++)
        {
            Destroy(spawnedCells[i].gameObject);
        }

        spawnedCells.Clear();

        floorPlan = new int[100];
        floorPlanCount = default;
        cellQueue = new Queue<int>();
        endRooms = new List<int>();
        bigRoomIndexes = new List<int>();
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
        endRooms.RemoveAll(item => bigRoomIndexes.Contains(item) || GetNeighborCount(item) > 1);
    }
    
    void SetupSpecialRooms()
    {
        BossRoomIndex = endRooms.Count > 0 ? endRooms[endRooms.Count - 1 ] : -1;

        if (BossRoomIndex != -1)
        {
            endRooms.RemoveAt(endRooms.Count - 1); 
        }

        itemRoomIndex = RandomEndRoom();
        shopRoomIndex = RandomEndRoom();

        if (itemRoomIndex == -1 || shopRoomIndex == -1 || BossRoomIndex == -1)
        {
            SetupDungeon();
            return;
        }
        
        ChangeSpecialVisuals();
        RoomManager.instance.SetupRooms(spawnedCells);
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

        if (floorPlan[index - 10] == 1 && !IsInSameRoom(index, index - 10))
            count++;
        if (floorPlan[index - 1] == 1 && !IsInSameRoom(index, index - 1))
            count++;
        if (floorPlan[index + 1] == 1 && !IsInSameRoom(index, index + 1))
            count++;
        if (floorPlan[index + 10] == 1 && !IsInSameRoom(index, index + 10))
            count++;
            
        return count;
    }
    
    private bool IsInSameRoom(int index1, int index2)
    {
        if (!indexToCellMap.ContainsKey(index1) || !indexToCellMap.ContainsKey(index2))
            return false;
            
        return indexToCellMap[index1] == indexToCellMap[index2];
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

        if(Random.value < 0.3f && index != 45)
        {
            foreach (var shape in roomShapes.OrderBy(_ => Random.value))
            {
                if(TryPlaceRoom(index, shape))
                {
                    return true;
                }
            }
        }

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
        Vector2 position = new Vector2(x * cellSize, -y * cellSize);

        Cell newCell = Instantiate(cellPrefab, position, Quaternion.identity);
        newCell.value = 1;
        newCell.index = index;
        newCell.cellList = new List<int> { index };
        newCell.setRoomShape(Cell.RoomShape.OnebyOne);
        newCell.setRoomType(Cell.RoomType.Regular);

        spawnedCells.Add(newCell);
        indexToCellMap[index] = newCell;
    }

    private bool TryPlaceRoom(int origin, int[] offsets)
    {
        List<int> currentRoomIndexes = new List<int>() {origin};

        foreach(var offset in offsets)
        {
            int currentIndexChecked = origin + offset;

            if (currentIndexChecked - 10 < 0 || currentIndexChecked + 10 >= floorPlan.Length)
                return false;

            if (floorPlan[currentIndexChecked] != 0)
                return false;

            if (currentIndexChecked == origin || currentIndexChecked % 10 == 0)
                continue; 
             
            currentRoomIndexes.Add(currentIndexChecked);
        }

        if (currentRoomIndexes.Count == 1)
            return false;

        foreach (int index in currentRoomIndexes)
        {
            floorPlan[index] = 1;
            floorPlanCount++;
            cellQueue.Enqueue(index);

            bigRoomIndexes.Add(index);
        }

        SpawnLargeRoom(currentRoomIndexes);

        return true;
    }

    private void SpawnLargeRoom(List<int> largeRoomIndexes)
    {
        Cell newCell = null;

        int combinedX = default;
        int combinedY = default;
        float offset = cellSize / 2f;

        for(int i = 0; i < largeRoomIndexes.Count; i++)
        {
            int x = largeRoomIndexes[i] % 10;
            int y = largeRoomIndexes[i] / 10;
            combinedX += x;
            combinedY += y;
        }

        if(largeRoomIndexes.Count == 4)
        {
            Vector2 position = new Vector2(combinedX / 4 * cellSize + offset, -combinedY / 4 * cellSize - offset);

            newCell = Instantiate(cellPrefab, position, Quaternion.identity);
            newCell.SetRoomSprite(largeRoom);
            newCell.setRoomShape(Cell.RoomShape.TwobyTwo);
        }

        if (largeRoomIndexes.Count == 2)
        {
            if (largeRoomIndexes[0] + 10 == largeRoomIndexes[1] || largeRoomIndexes[0] - 10 == largeRoomIndexes[1])
            {
                Vector2 position = new Vector2(combinedX / 2 * cellSize, -combinedY / 2 * cellSize - offset);
                newCell = Instantiate(cellPrefab, position, Quaternion.identity);
                newCell.SetRoomSprite(verticalRoom);
                newCell.setRoomShape(Cell.RoomShape.OnebyTwo);
            }
            else if (largeRoomIndexes[0] + 1 == largeRoomIndexes[1] || largeRoomIndexes[0] - 1 == largeRoomIndexes[1])
            {
                Vector2 position = new Vector2(combinedX / 2 * cellSize + offset, -combinedY / 2 * cellSize);
                newCell = Instantiate(cellPrefab, position, Quaternion.identity);
                newCell.SetRoomSprite(horizontalRoom);
                newCell.setRoomShape(Cell.RoomShape.TwobyOne);
            }
        }

        newCell.cellList = largeRoomIndexes ;
        newCell.cellList.Sort();
        newCell.index = newCell.cellList[0];

        spawnedCells.Add(newCell);
    }
}