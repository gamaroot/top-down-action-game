using Game;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private Material _wallMaterial;
    [SerializeField] private Material _floorMaterial;
    [SerializeField] private LayerMask _wallLayerMask;
    [SerializeField] private LayerMask _floorLayerMask;

    public void Generate(float roomSize, float wallHeight, Transform parent, out List<GameObject> waypoints)
    {
        int floorLayerIndex = Mathf.RoundToInt(Mathf.Log(this._floorLayerMask.value, 2));
        this.CreateFloor(roomSize, parent, floorLayerIndex);

        int wallLayerIndex = Mathf.RoundToInt(Mathf.Log(this._wallLayerMask.value, 2));

        // Left wall
        var leftWallPosition = new Vector3(-roomSize / 2f, wallHeight / 2f, 0);
        var leftWallScale = new Vector3(wallHeight, roomSize, 1);
        var leftWallRotation = Quaternion.Euler(0, 0, -90f);
        this.CreateWall(leftWallPosition, leftWallScale, leftWallRotation, parent, wallLayerIndex);

        // Right wall
        var rightWallPosition = new Vector3(roomSize / 2f, wallHeight / 2f, 0);
        var rightWallScale = new Vector3(wallHeight, roomSize, 1);
        var rightWallRotation = Quaternion.Euler(0, 0, 90f);
        this.CreateWall(rightWallPosition, rightWallScale, rightWallRotation, parent, wallLayerIndex);

        // Back wall
        var backWallPosition = new Vector3(0, wallHeight / 2f, -roomSize / 2f);
        var backWallScale = new Vector3(roomSize, wallHeight, 1);
        var backWallRotation = Quaternion.Euler(90f, 0, 0);
        this.CreateWall(backWallPosition, backWallScale, backWallRotation, parent, wallLayerIndex);

        // Front wall
        var frontWallPosition = new Vector3(0, wallHeight / 2f, roomSize / 2f);
        var frontWallScale = new Vector3(roomSize, wallHeight, 1);
        var frontWallRotation = Quaternion.Euler(-90f, 0, 0);
        this.CreateWall(frontWallPosition, frontWallScale, frontWallRotation, parent, wallLayerIndex);
        
        waypoints = this.CreateWaypoints(roomSize);

        Destroy(this);
    }

    private void CreateFloor(float roomSize, Transform parent, int layerMask)
    {
        var floor = new GameObject("Floor");
        floor.transform.SetParent(parent);
        floor.layer = layerMask;

        MeshFilter meshFilter = floor.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = floor.AddComponent<MeshRenderer>();

        meshRenderer.material = _floorMaterial;

        // Create a quad for the floor
        var mesh = new Mesh();
        var vertices = new Vector3[4]
        {
            new Vector3(-roomSize / 2f, 0, -roomSize / 2f), // Bottom left
            new Vector3(roomSize / 2f, 0, -roomSize / 2f), // Bottom right
            new Vector3(roomSize / 2f, 0, roomSize / 2f), // Top right
            new Vector3(-roomSize / 2f, 0, roomSize / 2f) // Top left
        };

        int[] triangles = new int[6]
        {
            0, 2, 1, // First triangle
            0, 3, 2  // Second triangle
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        floor.AddComponent<MeshCollider>();
    }

    private void CreateWall(Vector3 position, Vector3 scale, Quaternion rotation, Transform parent, int layerMask)
    {
        var wall = new GameObject("Wall");
        wall.transform.SetParent(parent, false);
        wall.layer = layerMask;

        MeshFilter meshFilter = wall.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = wall.AddComponent<MeshRenderer>();

        meshRenderer.material = _wallMaterial;

        // Create a quad for the wall
        var mesh = new Mesh();
        var vertices = new Vector3[4]
        {
            new Vector3(-0.5f * scale.x, 0, -0.5f * scale.y), // Bottom left
            new Vector3(0.5f * scale.x, 0, -0.5f * scale.y),  // Bottom right
            new Vector3(0.5f * scale.x, 0, 0.5f * scale.y),   // Top right
            new Vector3(-0.5f * scale.x, 0, 0.5f * scale.y)   // Top left
        };

        int[] triangles = new int[6]
        {
            0, 2, 1, // First triangle
            0, 3, 2  // Second triangle
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        wall.transform.position = position;
        wall.transform.rotation = rotation;
        wall.AddComponent<MeshCollider>();
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