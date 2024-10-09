using UnityEngine;

public class Room
{
    public Vector2Int position;
    public int width, height;
    public bool hasDoors;
    public bool isTreasureRoom;
    public bool isBossRoom;

    public Room(Vector2Int pos, int width, int height, bool hasDoors = true, bool isTreasureRoom = false, bool isBossRoom = false)
    {
        this.position = pos;
        this.width = width;
        this.height = height;
        this.hasDoors = hasDoors;
        this.isTreasureRoom = isTreasureRoom;
        this.isBossRoom = isBossRoom;
    }
}
