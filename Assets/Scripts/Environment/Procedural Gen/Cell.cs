using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public RoomType roomType;
    public Vector2 worldPosition;

    public enum RoomType
    {
        Regular,
        Item,
        Shop,
        Boss,
        Spawn
    }

    public int index;
    public int value;
    public int roomNum;

    public SpriteRenderer spriteRenderer;

    public void setRoomType(RoomType newRoomType)
    {
        roomType = newRoomType;
    }
    
    public void setSpecialRoomSprite(Sprite icon)
    {
        spriteRenderer.sprite = icon;
    }
}
