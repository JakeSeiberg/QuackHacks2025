using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public EdgeDirection3 direction;

    public void SetDoorType(Cell.RoomType fromRoomType, Cell.RoomType toRoomType, EdgeDirection3 dir)
    {
        direction = dir;

        if(spriteRenderer != null)
        {
            switch(dir)
            {
                case EdgeDirection3.Up:
                case EdgeDirection3.Down:
                    spriteRenderer.color = Color.white;
                    break;
                case EdgeDirection3.Left:
                case EdgeDirection3.Right:
                    spriteRenderer.color = Color.gray;
                    break;
            }
        }
    }
}
