using UnityEditor.AssetImporters;
using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void setDoorSprite(Sprite door)
    {
        spriteRenderer.sprite = door;
    }
}
