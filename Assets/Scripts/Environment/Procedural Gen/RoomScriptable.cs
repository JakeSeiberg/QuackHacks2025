using UnityEngine;

[CreateAssetMenu(fileName = "room", menuName = "Scriptable Objects/rooms")]
public class RoomScriptable : ScriptableObject
{
    public Cell.RoomType roomType;
    public Sprite[] roomVariations;
}
