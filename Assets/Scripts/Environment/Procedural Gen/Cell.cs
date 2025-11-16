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
        Boss
    }

    public int index;
    public int value;

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
