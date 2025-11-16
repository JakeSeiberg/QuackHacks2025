 using UnityEngine;
 using System.Collections.Generic;
using Unity.VisualScripting;

public class NewMonoBehaviourScript : MonoBehaviour
{  
    private int[] floorPlan;

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

    [SerializeField] private Sprite item;
    [SerializeField] private Sprite shop;
    [SerializeField] private Sprite boss;


    void Start()
    {
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
            if (index > 70) created |= VisitCell(index + 10);

            if (created == false)
                endRooms.Add(index);
        }
        if (floorPlanCount < minRooms)
        {
            SetupDungeon();
            return;
        }

        SetupSpecialRooms();
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
    }
    void ChangeSpecialVisuals()
    {
        foreach (var cell in spawnedCells)
        {
            if(cell.index == itemRoomIndex)
            {
                cell.setSpecialRoomSprite(item);
            }
            if(cell.index == shopRoomIndex)
            {
                cell.setSpecialRoomSprite(shop);
            }
            if(cell.index == BossRoomIndex)
            {
                cell.setSpecialRoomSprite(boss);
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

        endRooms.Remove(RandomRoom);

        return index;
    }
    private int GetNeighborCount(int index)
    {
        return floorPlan[index - 10] + floorPlan[index - 1 ] + floorPlan[index + 1] + floorPlan[index + 10];
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
        Vector2 position = new Vector2(x * cellSize, -y * cellSize);

        Cell newCell = Instantiate(cellPrefab, position, Quaternion.identity);
        newCell.value = 1;
        newCell.index = index;

        spawnedCells.Add(newCell); 
    }
}
