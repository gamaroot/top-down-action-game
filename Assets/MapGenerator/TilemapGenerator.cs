using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapGenerator : MonoBehaviour
{
    public GameObject floorPrefab;         
    public GameObject wallPrefab;          
    public GameObject doorPrefab;          
    public GameObject trapPrefab;          
    public GameObject obstaclePrefab;
    public GameObject treasureFloorPrefab;
    public GameObject enemySpawnerPrefab;
    public GameObject treasurePrefab;

    public int roomWidth = 10;
    public int roomHeight = 10;
    public int numRooms = 10;              
    public int bossRoomSizeMultiplier = 2;

    private List<Room> rooms = new List<Room>();
    private Dictionary<Vector2Int, Room> roomMap = new Dictionary<Vector2Int, Room>(); // Tracks room positions
    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        Vector2Int currentPos = Vector2Int.zero; // Start at (0, 0)
        Room bossRoom = null;

        for (int i = 0; i < numRooms; i++)
        {
            bool isTreasureRoom = (i == 1); // The second room generated is the treasure room.
            bool isBossRoom = (i == numRooms - 1); // The last room generated will be the boss room.

            Room newRoom = new Room(currentPos, roomWidth, roomHeight, !isTreasureRoom && !isBossRoom, isTreasureRoom, isBossRoom);

            // For boss room, scale it up
            if (isBossRoom)
            {
                newRoom.width *= bossRoomSizeMultiplier;
                newRoom.height *= bossRoomSizeMultiplier;
                bossRoom = newRoom; // Store the boss room for middle placement later
            }

            // Add room to the list and map
            rooms.Add(newRoom);
            roomMap[currentPos] = newRoom;

            // Generate the room
            GenerateRoom(newRoom);

            // Move to a random adjacent position for the next room
            currentPos = GetRandomNextRoomPosition(currentPos);
        }

        // Move the boss room to the middle after all other rooms are generated
        Vector2Int middlePos = new Vector2Int(roomWidth * bossRoomSizeMultiplier, roomHeight * bossRoomSizeMultiplier);
        bossRoom.position = middlePos;
        GenerateRoom(bossRoom, true); // Re-generate boss room in the middle

        // Connect rooms with doors
        ConnectRooms();
    }

    void GenerateRoom(Room room, bool isBossRoom = false)
    {
        // Choose the correct floor prefab for this room
        GameObject chosenFloorPrefab = room.isTreasureRoom ? treasureFloorPrefab : floorPrefab;

        // Generate floor and walls for the room
        for (int x = room.position.x; x < room.position.x + room.width; x++)
        {
            for (int y = room.position.y; y < room.position.y + room.height; y++)
            {
                // Place floor in every tile
                Vector3 floorPosition = new Vector3(x, 0, y);  // Place floor on the x, z plane (3D)
                Instantiate(chosenFloorPrefab, floorPosition, Quaternion.identity);

                // Place walls at the room's edges
                if (x == room.position.x || x == room.position.x + room.width - 1 || y == room.position.y || y == room.position.y + room.height - 1)
                {
                    Vector3 wallPosition = new Vector3(x, 1, y);  // Place walls slightly higher in Y axis
                    Instantiate(wallPrefab, wallPosition, Quaternion.identity);
                }
            }
        }

        // Add specific elements to the room
        if (room.isTreasureRoom)
        {
            AddTreasure(room);
        }
        else if (room.isBossRoom)
        {
            AddBossRoomFeatures(room);
        }
        else
        {
            AddEnemySpawner(room);
            PlaceTrapsAndObstacles(room);
        }
    }

    void AddTreasure(Room room)
    {
        // Place a treasure in the center of the treasure room
        Vector3 treasurePosition = new Vector3(room.position.x + room.width / 2, 0.5f, room.position.y + room.height / 2);
        Instantiate(treasurePrefab, treasurePosition, Quaternion.identity);
    }

    void AddEnemySpawner(Room room)
    {
        // Place an enemy spawner in a random position inside the room
        Vector3 spawnerPosition = new Vector3(room.position.x + room.width / 2, 0.5f, room.position.y + room.height / 2);
        Instantiate(enemySpawnerPrefab, spawnerPosition, Quaternion.identity);
    }

    void AddBossRoomFeatures(Room room)
    {
        // Place traps in the boss room randomly
        PlaceTrapsAndObstacles(room);

        // Add additional boss-specific objects if necessary
    }

    void PlaceTrapsAndObstacles(Room room)
    {
        int numObstacles = Random.Range(2, 5); // Random number of obstacles in the room
        for (int i = 0; i < numObstacles; i++)
        {
            int randX = Random.Range(room.position.x + 1, room.position.x + room.width - 1);
            int randY = Random.Range(room.position.y + 1, room.position.y + room.height - 1);

            Vector3 obstaclePosition = new Vector3(randX, 0.5f, randY); // Adjust Y position slightly higher
            Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
        }

        int numTraps = Random.Range(1, 3); // Random number of traps in the room
        for (int i = 0; i < numTraps; i++)
        {
            int randX = Random.Range(room.position.x + 1, room.position.x + room.width - 1);
            int randY = Random.Range(room.position.y + 1, room.position.y + room.height - 1);

            Vector3 trapPosition = new Vector3(randX, 0.5f, randY); // Slightly above ground
            Instantiate(trapPrefab, trapPosition, Quaternion.identity);
        }
    }

    Vector2Int GetRandomNextRoomPosition(Vector2Int currentPos)
    {
        // Get a random direction to move and calculate the next room position
        Vector2Int direction = directions[Random.Range(0, directions.Length)];
        Vector2Int nextPos = currentPos + direction * roomWidth;

        // If the next position already has a room, try another direction
        if (roomMap.ContainsKey(nextPos))
        {
            return GetRandomNextRoomPosition(currentPos);
        }

        return nextPos;
    }

    void ConnectRooms()
    {
        // Iterate over all rooms and add doors to connect adjacent rooms
        foreach (Room room in rooms)
        {
            // Treasure rooms can only have one door, so we skip multi-door logic for them
            if (room.isTreasureRoom)
            {
                AddSingleDoorToTreasureRoom(room);
            }
            else
            {
                foreach (Vector2Int dir in directions)
                {
                    Vector2Int neighborPos = room.position + dir * roomWidth;

                    // Check if the neighbor position is valid and if it's a valid door position
                    if (roomMap.ContainsKey(neighborPos))
                    {
                        AddDoor(room.position + dir * roomWidth / 2); // Add door at the midpoint between the rooms
                    }
                    else if (!IsCornerRoom(room.position, dir)) // Ensure corners don't have doors if there is no adjacent room
                    {
                        AddDoor(room.position + dir * roomWidth / 2); // This adds a door even if no adjacent room is present
                    }
                }
            }
        }
    }

    void AddSingleDoorToTreasureRoom(Room treasureRoom)
    {
        // Find a single adjacent room and add one door
        List<Vector2Int> availableDirections = new List<Vector2Int>();

        // Check all directions to see where a neighboring room is
        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = treasureRoom.position + dir * roomWidth;
            if (roomMap.ContainsKey(neighborPos))
            {
                availableDirections.Add(dir);
            }
        }

        // Randomly select one direction and add a door there if there's at least one available
        if (availableDirections.Count > 0)
        {
            Vector2Int chosenDirection = availableDirections[Random.Range(0, availableDirections.Count)];
            AddDoor(treasureRoom.position + chosenDirection * roomWidth / 2); // Place the door at the midpoint
        }
    }

    void AddDoor(Vector2Int position)
    {
        Vector3 doorPosition = new Vector3(position.x, 0.5f, position.y);
        Instantiate(doorPrefab, doorPosition, Quaternion.identity);
    }

    bool IsCornerRoom(Vector2Int roomPosition, Vector2Int direction)
    {
        // Determine if this position is a corner room with no adjacent rooms
        Vector2Int checkPosition = roomPosition + direction * roomWidth;
        return !roomMap.ContainsKey(checkPosition);
    }
}


