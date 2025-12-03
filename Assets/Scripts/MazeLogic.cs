using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MazeLogic : MonoBehaviour
{
    [Header("Character Configurations")]
    public GameObject character;
    public List<GameObject> enemyCharacter;
    public int enemyCount;

    [Header("Lighting Configurations")]
    public GameObject lampPrefab;
    public int lampCount;

    [Header("Size Options")]
    public int width;
    public int depth;
    public float scale;

    [Header("Room Configurations")]
    public int roomCount;
    public int roomMinSize;
    public int roomMaxSize;

    [Header("Objects Setup")]
    public NavMeshSurface surface;
    public List<GameObject> buildingObjects;

    // Private Variables
    public byte[,] map;
    GameObject[,] buildingList;
    private float height = 0.5f;

    void InitialiseMap()
    {
        map = new byte[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1;
                // Debug.Log("Setting map[" + x + ", " + z + "] = " + map[x, z]);
            }
        }
    }

    public virtual void GenerateMaps()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0, 100) < 50)
                    map[x, z] = 0;

                // Debug.Log("Setting map[" + x + ", " + z + "] = " + map[x, z]);
            }
        }
    }

    void DrawMaps()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    Vector3 position = new Vector3(x, height, z);
                    GameObject wall = Instantiate(buildingObjects[Random.Range(0, buildingObjects.Count)], position, Quaternion.identity);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }
    }

    public virtual void PlaceCharacter()
    {
        bool playerSet = false;

        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x = Random.Range(0, width);
                int z = Random.Range(0, depth);

                if (map[x, z] == 2 && !playerSet)
                {
                    Debug.Log("Placing Character ...");
                    playerSet = true;
                    character.transform.position = new Vector3(x * scale, 0, z * scale);
                }
                else if (playerSet)
                {
                    Debug.Log("Character already placed!");
                }
            }
        }
    }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;

        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;

        if (map[x + 1, z] == 0) count++;
        if (map[x - 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;

        return count;
    }

    public virtual void PlaceEnemy()
    {
        int enemySet = 0;

        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x = Random.Range(0, width);
                int z = Random.Range(0, depth);

                if (map[x, z] == 2 && enemySet != enemyCount)
                {
                    Debug.Log("Placing enemies ...");
                    enemySet++;
                    GameObject enemy = Instantiate(enemyCharacter[Random.Range(0, enemyCharacter.Count)], new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                }
                else if (enemySet == enemyCount)
                {
                    Debug.Log("Already placing enemies!!");
                    return;
                }
            }
        }
    }

    public void PlaceWallLamps()
    {
        int lampsPlaced = 0;

        while (lampsPlaced < lampCount)
        {
            int x = Random.Range(1, width - 1); // Avoid placing lamps at the edges
            int z = Random.Range(1, depth - 1);

            // Check if the tile is a wall (assuming 1 is the wall tile code)
            if (map[x, z] == 1)
            {
                // Find a valid side to attach the lamp
                Vector3 position = Vector3.zero;
                Quaternion rotation = Quaternion.identity;
                bool validSpot = false;

                // Check surrounding tiles to determine lamp orientation
                if (x > 0 && map[x - 1, z] == 0) // Open space to the left
                {
                    position = new Vector3(x * scale - scale / 2, height, z * scale); // Offset towards the left wall
                    rotation = Quaternion.Euler(90, 0, 0); // Rotate to face right
                    validSpot = true;
                }
                else if (x < width - 1 && map[x + 1, z] == 0) // Open space to the right
                {
                    position = new Vector3(x * scale + scale / 2, height, z * scale); // Offset towards the right wall
                    rotation = Quaternion.Euler(-90, 0, 0); // Rotate to face left
                    validSpot = true;
                }
                else if (z > 0 && map[x, z - 1] == 0) // Open space below
                {
                    position = new Vector3(x * scale, height, z * scale - scale / 2); // Offset towards the bottom wall
                    rotation = Quaternion.Euler(0, 0, 0); // Face forward
                    validSpot = true;
                }
                else if (z < depth - 1 && map[x, z + 1] == 0) // Open space above
                {
                    position = new Vector3(x * scale, height, z * scale + scale / 2); // Offset towards the top wall
                    rotation = Quaternion.Euler(180, 0, 0); // Face backward
                    validSpot = true;
                }

                // If a valid spot is found, place the lamp
                if (validSpot)
                {
                    Instantiate(lampPrefab, position, rotation);
                    lampsPlaced++;

                    // Debug.Log($"Wall Lamp placed at: {x}, {z}");
                    // Debug.Log($"Wall Lamp rotated at: {rotation.x}, {rotation.z}");
                }
            }
        }
    }

    public virtual void AddRooms(int count, int min, int max)
    {
        for (int c = 0; c < count; c++)
        {
            int startX = Random.Range(3, width - 3);
            int startZ = Random.Range(3, depth - 3);
            int roomWidth = Random.Range(min, max);
            int roomDepth = Random.Range(min, max);

            for (int x = startX; x < width - 3 && x < startX + roomWidth; x++)
            {
                for (int z = startZ; z < depth - 3 && z < startZ + roomDepth; z++)
                {
                    map[x, z] = 2;
                }
            }
        }
    }

    void Start()
    {
        InitialiseMap();
        GenerateMaps();
        AddRooms(roomCount, roomMinSize, roomMaxSize);
        DrawMaps();
        PlaceCharacter();
        PlaceEnemy();
        PlaceWallLamps();
        surface.BuildNavMesh();
    }

    void Update()
    {

    }
}
