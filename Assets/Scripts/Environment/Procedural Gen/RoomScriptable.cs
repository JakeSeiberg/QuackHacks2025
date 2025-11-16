using UnityEngine;

[CreateAssetMenu(fileName = "room", menuName = "Scriptable Objects/rooms")]
public class RoomScriptable : ScriptableObject
{
    public Cell.RoomType roomType;

    // Optional: different looks for the same type (e.g. 3 item rooms)
    public Sprite[] roomVariations;
}
