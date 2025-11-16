using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public EdgeDirection direction;

    public void SetDoorType(Cell.RoomType fromRoomType, Cell.RoomType toRoomType, EdgeDirection dir)
    {
        direction = dir;

        // Optional: set sprite based on room type combination
        // You can assign different door sprites in the inspector
        if(spriteRenderer != null)
        {
            switch(dir)
            {
                case EdgeDirection.Up:
                case EdgeDirection.Down:
                    spriteRenderer.color = Color.white; // placeholder
                    break;
                case EdgeDirection.Left:
                case EdgeDirection.Right:
                    spriteRenderer.color = Color.gray; // placeholder
                    break;
            }
        }
    }
}
