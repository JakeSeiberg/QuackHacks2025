using UnityEngine;


[CreateAssetMenu(fileName = "door", menuName = "Scriptable Objects/doors")]
public class DoorScriptable : ScriptableObject
{
    public Cell.RoomType roomType;
    public Sprite upDoor;
    public Sprite downDoor;
    public Sprite leftDoor;
    public Sprite rightDoor;

}
