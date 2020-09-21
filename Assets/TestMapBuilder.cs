using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMapBuilder : MonoBehaviour
{
    public Transform walkableContainer;
    public Transform unwalkableContainer;
    public WorldTileDefinition defaultFloorTile;
    public WorldTileDefinition houseTile;

    public int worldSizeX;
    public int worldSizeY;
    public int worldSizeLevels;
    WorldTileData[, ,] worldTileData;

    void Start() {
        GenerateTileData();
        SpawnTileMap();
        // // Spawn floor
        // for (int x = 0; x<worldSizeX; x++) {
        //     for (int y = 0; y<worldSizeY; y++) {
        //         GameObject newTile = Instantiate(defaultTile, new Vector3(x, 0f, y), Quaternion.identity) as GameObject;
        //         if (y!=5 && x!=5)
        //             newTile.transform.SetParent(surface.transform);
        //     }
        // }

        // // Spawn house
        // GameObject newHouseTile = Instantiate(houseTile, new Vector3(5, 1, 5), Quaternion.identity);
        // newHouseTile.transform.SetParent(surface.transform);

        // surface.BuildNavMesh();
    }

    void GenerateTileData() {
        worldTileData = new WorldTileData[worldSizeX, worldSizeY, worldSizeLevels];

        // Generate the floor
        for (int x = 0; x < worldSizeX; x++) {
            for (int y = 0; y < worldSizeY; y++) {
                AddTile(x, y, 0, new WorldTileData(defaultFloorTile));
            }
        }

        // Place a house at 3 random points
        for (int i = 0; i<3; i++) {
            AddTile(Random.Range(0, worldSizeX), Random.Range(0, worldSizeY), 1, new WorldTileData(houseTile));
        }
    }

    void AddTile(int x, int y, int level, WorldTileData tileData) {
        // Add the new tile data
        worldTileData[x, y, level] = tileData;

        // If we place an unwalkable tile, all the underneath tiles need to be unwalkable as well
        if (!tileData.walkable) {
            for (int i = level; i >= 0; i--) {
                worldTileData[x, y, i].walkable = false;
            }
        }
    }

    void SpawnTileMap() {
        for (int i = 0; i < worldSizeLevels; i++) {
            for (int x = 0; x < worldSizeX; x++) {
                for (int y = 0; y < worldSizeY; y++) {

                    if (worldTileData[x, y, i] == null) {
                        continue;
                    }

                    GameObject tile = Instantiate(worldTileData[x, y, i].prefab, new Vector3(x, i, y), Quaternion.identity) as GameObject;

                    if (worldTileData[x, y, i].walkable) {
                        tile.transform.SetParent(walkableContainer);
                    } else {
                        tile.transform.SetParent(unwalkableContainer);
                    }
                }
            }
        }
        walkableContainer.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    class WorldTileData {
        public string nameID;
        public bool walkable;
        public GameObject prefab;

        public WorldTileData(WorldTileDefinition tileDefinition) {
            nameID = tileDefinition.nameID;
            walkable = tileDefinition.walkable;
            prefab = tileDefinition.prefab;
        }
    }
}
