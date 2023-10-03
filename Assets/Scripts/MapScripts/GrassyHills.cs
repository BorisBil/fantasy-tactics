using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class GrassyHills : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileSets.GrassyHillTiles[] grassyHills;
    public PropSets.GrassyHillProps[] grassyHillsProps;

    private int[,,] tileTypeMap;
    private int[,,] propTypeMap;

    private List<Vector3Int> tileMapList;
    private List<Vector3Int> graphNodeList;
    private List<Vector3Int> propTypeList;
    private List<Prop> propList;

    public GameObject map;
    public GameObject tiles;
    public GameObject props;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /* MAP DATA GENERATION FUNCTION
     * This function creates and stores map data into several arrays and lists for game map usage
     */
    public void GenerateMapData(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        tileTypeMap = new int[mapSizeX, mapSizeY, mapSizeZ];
        tileMapList = new List<Vector3Int>();

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (tileMapList.Contains(new Vector3Int(x, y, 0)))
                {
                    continue;
                }

                float rollTerrainChance = Random.Range(0.0f, 1.0f);
                float rollElevationChance = Random.Range(0.0f, 1.0f);

                if (rollTerrainChance <= 0.9f && x < mapSizeX - 2 && y < mapSizeY - 2)
                {
                    tileTypeMap[x, y, 0] = 0;
                    tileMapList.Add(new Vector3Int(x, y, 0));

                    if (rollElevationChance > 0.90f && x < mapSizeX - 2 && y < mapSizeY - 2)
                    {
                        tileTypeMap[x, y + 1, 1] = 0;
                        tileTypeMap[x, y + 2, 1] = 0;
                        tileTypeMap[x + 1, y + 1, 1] = 0;
                        tileTypeMap[x + 1, y + 2, 1] = 0;

                        tileMapList.Add(new Vector3Int(x, y + 1, 1));
                        tileMapList.Add(new Vector3Int(x, y + 2, 1));
                        tileMapList.Add(new Vector3Int(x + 1, y + 1, 1));
                        tileMapList.Add(new Vector3Int(x + 1, y + 2, 1));

                        float rollMountainChance = Random.Range(0.0f, 1.0f);
                        if (rollMountainChance > 0.9f)
                        {
                            tileTypeMap[x, y + 2, 2] = 2;
                            tileTypeMap[x + 1, y + 2, 2] = 2;

                            tileMapList.Add(new Vector3Int(x, y + 2, 2));
                            tileMapList.Add(new Vector3Int(x + 1, y + 2, 2));
                        }
                    }
                }
                else
                {
                    tileTypeMap[x, y, 0] = 0;
                    tileMapList.Add(new Vector3Int(x, y, 0));
                }
                
                if (x < mapSizeX - 2 && y < mapSizeY - 2)
                {
                    if (rollTerrainChance > 0.9f && rollTerrainChance < 1.0f)
                    {
                        tileTypeMap[x, y, 0] = 1;
                        tileTypeMap[x + 1, y, 0] = 1;
                        tileTypeMap[x, y + 1, 0] = 1;
                        tileTypeMap[x + 1, y + 1, 0] = 1;

                        tileMapList.Add(new Vector3Int(x, y, 0));
                        tileMapList.Add(new Vector3Int(x + 1, y, 0));
                        tileMapList.Add(new Vector3Int(x, y + 1, 0));
                        tileMapList.Add(new Vector3Int(x + 1, y + 1, 0));
                    }
                }
            }
        }
    }
    
    /* GENERATE PROP DATA
     * This function is responsible for generating the random cover and props on the map
     */

    public void GeneratePropData(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        propTypeMap = new int[mapSizeX, mapSizeY, mapSizeZ];
        propTypeList = new List<Vector3Int>();
        propList = new List<Prop>();

        foreach (var item in tileMapList)
        {
            if (!tileMapList.Contains(new Vector3Int(item.x, item.y, item.z + 1))
                && !propTypeList.Contains(new Vector3Int(item.x, item.y, item.z)))
            {
                float propChance = Random.Range(0.0f, 1.0f);

                if (propChance > 0.95f)
                {
                    float propTypeChance = Random.Range(0.0f, 1.0f);
                    
                    if (propTypeChance < 0.3)
                    {
                        propTypeMap[item.x, item.y, item.z] = 0;
                        propTypeList.Add(new Vector3Int(item.x, item.y, item.z));
                    }

                    if (propTypeChance > 0.3 && propTypeChance < 0.8)
                    {
                        propTypeMap[item.x, item.y, item.z] = 1;
                        propTypeList.Add(new Vector3Int(item.x, item.y, item.z));
                    }

                    if (propTypeChance > 0.8)
                    {
                        if (item.x < mapSizeX - 2
                            && tileMapList.Contains(new Vector3Int(item.x + 1, item.y, item.z))
                            && tileMapList.Contains(new Vector3Int(item.x + 2, item.y, item.z))
                            && !tileMapList.Contains(new Vector3Int(item.x + 2, item.y, item.z + 1))
                            && !tileMapList.Contains(new Vector3Int(item.x + 2, item.y, item.z + 1)))
                        {
                            propTypeMap[item.x, item.y, item.z] = 2;
                            propTypeMap[item.x + 1, item.y, item.z] = 3;
                            propTypeMap[item.x + 2, item.y, item.z] = 4;
                            propTypeList.Add(new Vector3Int(item.x, item.y, item.z));
                            propTypeList.Add(new Vector3Int(item.x + 1, item.y, item.z));
                            propTypeList.Add(new Vector3Int(item.x + 2, item.y, item.z));
                        }
                    }
                }
            }
        }
    }

    /* GENERATE THE MAP
     * This function is responsible for generating the actual map visuals itself
     */
    public void GenerateMapVisual()
    {
        foreach (var item in tileMapList)
        {
            /// Instantiating the tiles after setting their type based on the map graph
            TileSets.GrassyHillTiles type = grassyHills[tileTypeMap[item.x, item.y, item.z]];
            GameObject tile = Instantiate(type.tileVisualPrefab, new Vector3(item.x, item.y, item.z), Quaternion.identity);
            tile.transform.parent = tiles.transform;

            /// Setting clickable tiles based on type
            Tile clickableTile = tile.GetComponent<Tile>();
            clickableTile.tileLocation = item;
            if (tileTypeMap[item.x, item.y, item.z] < 3)
            {
                clickableTile.isClickable = true;
            }
        }

        foreach (var item in propTypeList)
        {
            /// Instantiating the props after setting their type based on the prop graph
            PropSets.GrassyHillProps type = grassyHillsProps[tileTypeMap[item.x, item.y, item.z]];
            GameObject prop = Instantiate(type.propVisualPrefab, new Vector3(item.x, item.y, item.z), Quaternion.identity);
            prop.transform.parent = props.transform;

            /// Setting props based on type
            Prop propInfo = prop.GetComponent<Prop>();
            
            propInfo.location = item;
            propInfo.isStructure = type.isStructure;
            propInfo.blocksTile = type.blocksTile;
            propInfo.coverType = type.coverType;

            propList.Add(propInfo);
        }
    }

    /* MAP GRAPH
     * This function creates nodes on all of the tiles and defines neighbors for pathfinding usage
     */
    public Node[,,] GenerateMapGraph(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        Node[,,] graph;

        graph = new Node[mapSizeX, mapSizeY, mapSizeZ];
        graphNodeList = new List<Vector3Int>();

        /// Generate a node for each tile we have on the map
        foreach (var item in tileMapList)
        {
            graph[item.x, item.y, item.z] = new Node();
            graph[item.x, item.y, item.z].movementCost =
                grassyHills[tileTypeMap[item.x, item.y, item.z]].movementCost;

            /// Setting unwalkable tiletypes to not be walkable on the node grid
            if (grassyHills[tileTypeMap[item.x, item.y, item.z]].isWalkable == true)
            {
                graph[item.x, item.y, item.z].isWalkable = true;
            }
            else
            {
                graph[item.x, item.y, item.z].isWalkable = false;
            }

            /// Setting "underground" nodes to be unwalkable
            if (tileMapList.Contains(new Vector3Int(item.x, item.y, item.z + 1)))
            {
                graph[item.x, item.y, item.z].isWalkable = false;
            }

            graph[item.x, item.y, item.z].location = new Vector3Int(item.x, item.y, item.z);
            graphNodeList.Add(item);
        }

        foreach (var index in graphNodeList)
        {
            /// Adding all the neighbors that are on the same plane
            if (tileMapList.Contains(new Vector3Int(index.x - 1, index.y, index.z)))
            {
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x - 1, index.y, index.z]);
            }

            if (tileMapList.Contains(new Vector3Int(index.x + 1, index.y, index.z)))
            {
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x + 1, index.y, index.z]);
            }

            if (tileMapList.Contains(new Vector3Int(index.x, index.y - 1, index.z)))
            {
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x, index.y - 1, index.z]);
            }

            if (tileMapList.Contains(new Vector3Int(index.x, index.y + 1, index.z)))
            {
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x, index.y + 1, index.z]);
            }

            /// Adding "steps" between higher/lower ground to the neighbors
            if (tileMapList.Contains(new Vector3Int(index.x - 1, index.y, index.z - 1)))
            {
                if (graph[index.x - 1, index.y, index.z - 1].isWalkable)
                {
                    graph[index.x, index.y, index.z].neighbors.Add(graph[index.x - 1, index.y, index.z - 1]);
                    graph[index.x - 1, index.y, index.z - 1].neighbors.Add(graph[index.x, index.y, index.z]);
                }
            }

            if (tileMapList.Contains(new Vector3Int(index.x + 1, index.y, index.z - 1)))
            {
                if (graph[index.x + 1, index.y, index.z - 1].isWalkable)
                {
                    graph[index.x, index.y, index.z].neighbors.Add(graph[index.x + 1, index.y, index.z - 1]);
                    graph[index.x + 1, index.y, index.z - 1].neighbors.Add(graph[index.x, index.y, index.z]);
                }
            }

            if (tileMapList.Contains(new Vector3Int(index.x, index.y - 1, index.z - 1)))
            {
                if (graph[index.x, index.y - 1, index.z - 1].isWalkable)
                {
                    graph[index.x, index.y, index.z].neighbors.Add(graph[index.x, index.y - 1, index.z - 1]);
                    graph[index.x, index.y - 1, index.z - 1].neighbors.Add(graph[index.x, index.y, index.z]);
                }
            }

            if (tileMapList.Contains(new Vector3Int(index.x, index.y + 1, index.z - 1)))
            {
                if (graph[index.x, index.y + 1, index.z - 1].isWalkable)
                {
                    graph[index.x, index.y, index.z].neighbors.Add(graph[index.x, index.y + 1, index.z - 1]);
                    graph[index.x, index.y + 1, index.z - 1].neighbors.Add(graph[index.x, index.y, index.z]);
                }
            }
        }

        foreach (Prop prop in propList)
        {
            if (prop.blocksTile)
            {
                graph[prop.location.x, prop.location.y, prop.location.z].isWalkable = false;
            }
        }

        return graph;
    }
}
