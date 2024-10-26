using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class WorldGeneration : MonoBehaviour
{
    [Header("World")]
    [SerializeField] private Tilemap world;
    [SerializeField] private List<RuleTile> tiles = new List<RuleTile>();

    [Space][Header("Generation Options")]
    [SerializeField] float width;
    [SerializeField] private float maxHeight;
    [SerializeField] private float smoothness;
    [SerializeField] private int seed;
    [SerializeField] private int dirtlayer = 5;

    [Space][Header("Other")]
    [SerializeField] private Transform playerPos;
    enum TILES
    {
        Grass = 0,
        Dirt = 1,
        Stone = 2
    }

    void Start()
    {
        seed = Random.Range(-1000000, 1000000);
        dirtlayer = Random.Range(10, 16);
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        for (int x = 0; x < width; x++)
        {
            int height = Mathf.RoundToInt(maxHeight * Mathf.PerlinNoise(x / smoothness, seed));

            for (int y = 0; y < maxHeight; y++)
            {
                if (y < height - dirtlayer) //stone
                    world.SetTile(new Vector3Int(x, y, 0), tiles[(int)TILES.Stone]);
                else if (y < height) //dirt
                    world.SetTile(new Vector3Int(x, y, 0), tiles[(int)TILES.Dirt]);
                else if (y == height && world.GetTile(new Vector3Int(x, y - 1, 0)) == tiles[(int)TILES.Dirt]) //grass
                {
                    world.SetTile(new Vector3Int(x, y, 0), tiles[(int)TILES.Grass]);
                }


                if (x == Mathf.RoundToInt(width / 2))
                {
                    playerPos.transform.position = new Vector2(x, height + 5);
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            world.ClearAllTiles();
            GenerateWorld();
        }
    }
}