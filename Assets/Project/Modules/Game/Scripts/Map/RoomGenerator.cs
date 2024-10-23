using Game;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _parent;

    // 0: front, 1: back, 2: left, 3: right
    [SerializeField] private GameObject[] _walls;
    [SerializeField] private GameObject[] _doors;
    [SerializeField] private GameObject[] _wallsWithDoor;

    public Room Generate(RoomData data)
    {
        base.transform.localScale = new Vector3(data.SquaredSize, data.WallHeight, data.SquaredSize);
        base.transform.position = new Vector3(data.Position.x, (data.WallHeight - 1f) / 2f, data.Position.y);

        List<GameObject> existingDoors = new();
        for (int index = 0; index < this._walls.Length; index++)
        {
            int neighborId = data.NeighborsID[index];
            bool hasNeighbor = neighborId != -1;

            GameObject wall = this._walls[index];
            wall.SetActive(!hasNeighbor);
            
            GameObject wallWithDoor = this._wallsWithDoor[index];
            wallWithDoor.SetActive(hasNeighbor);

            GameObject door = this._doors[index];
            if (hasNeighbor && neighborId < data.Id)
                door.SetActive(false);
            else
                existingDoors.Add(door);
        }

        Destroy(this);

        return base.gameObject.GetComponent<Room>()
                                .Init(data.Id,
                                      existingDoors.ToArray(), 
                                      this.CreateWaypoints(data.SquaredSize), 
                                      this._parent, 
                                      new SpawnerGenerator().GenerateSpawnConfig(data));
    }

    private List<GameObject> CreateWaypoints(float roomSize)
    {
        var waypointsParent = new GameObject("Waypoints");
        waypointsParent.transform.SetParent(this.transform, false);

        float roomCorner = roomSize / 2.5f;

        // Define the four corners of the room
        Vector3[] positions = {
                new(-roomCorner, 0, -roomCorner), // Bottom left
                new(roomCorner, 0, -roomCorner),  // Bottom right
                new(roomCorner, 0, roomCorner),   // Top right
                new(-roomCorner, 0, roomCorner)   // Top left
            };

        var waypoints = new List<GameObject>();
        for (int index = 0; index < 4; index++)
        {
            var waypoint = new GameObject($"Waypoint #{index}");
            waypoint.transform.SetParent(waypointsParent.transform, false);
            waypoint.transform.position = positions[index];
            waypoints.Add(waypoint);
        }
#if UNITY_EDITOR
        waypointsParent.AddComponent<DebugWaypoints>();
#endif
        return waypoints;
    }
}