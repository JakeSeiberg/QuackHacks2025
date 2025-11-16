using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public RoomType roomType;
    public RoomShape roomShape;

    public enum RoomType
    {
        Regular,
        Item,
        Shop,
        Boss
    }

    public enum RoomShape
    {
        OnebyOne,
        OnebyTwo,
        TwobyOne,
        TwobyTwo 
    }

    public int index;
    public int value;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer roomSprite;
    public List<int> cellList = new List<int>();

    public void setRoomType(RoomType newRoomType)
    {
        roomType = newRoomType;
    }
    public void setRoomShape(RoomShape newRoomShape)
    {
        roomShape = newRoomShape;
    }
    
    public void setSpecialRoomSprite(Sprite icon)
    {
        spriteRenderer.sprite = icon;
    }
    
    public void SetRoomSprite(Sprite roomIcon)
    {
        roomSprite.sprite = roomIcon;
    }
}