using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public EdgeDirection direction;

    public void SetDoorType(Cell.RoomType fromRoomType, Cell.RoomType toRoomType, EdgeDirection dir)
    {
        direction = dir;

        if(spriteRenderer != null)
        {
            switch(dir)
            {
                case EdgeDirection.Up:
                case EdgeDirection.Down:
                    spriteRenderer.color = Color.white;
                    break;
                case EdgeDirection.Left:
                case EdgeDirection.Right:
                    spriteRenderer.color = Color.gray;
                    break;
            }
        }
    }
}
