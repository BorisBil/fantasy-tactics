using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PropSets;
using static UnityEditor.Progress;

/// 
/// THIS GENERATES THE DATA AND SPAWNS IN THE MAP FOR THE GRASSYHILLS ENVIRONMENT
///

public class GrassyHills : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileSets.GrassyHillTiles[] grassyHills;
    public PropSets.GrassyHillProps[] grassyHillsProps;

    private Dictionary<Vector3, TileSets.GrassyHillTiles> tileTypeMap;
    private Dictionary<Vector3, GrassyHillProps> propTypeMap;
    private Dictionary<Vector3, Tile> tileMapDict;
    
    private Dictionary<Vector3, Node> pathfindingGraph;
    private Dictionary<Vector3, TileLight> lightMap;
    
    private List<Prop> propList;
    private List<Node> walkableNodes;

    public GameObject map;
    public GameObject tiles;
    public GameObject props;
    public GameObject tilelights;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /* MAP DATA GENERATION FUNCTION
     * This function creates and stores map data into several arrays and lists for game map usage
     */
    public void GenerateMapData(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        tileTypeMap = new Dictionary<Vector3, TileSets.GrassyHillTiles>();

        List<Vector3> tilesToRemove = new List<Vector3>();

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (tileTypeMap.ContainsKey(new Vector3(x, y, 0)))
                {
                    continue;
                }

                float rollTerrainChance = Random.Range(0.0f, 1.0f);
                float rollElevationChance = Random.Range(0.0f, 1.0f);

                if (rollTerrainChance <= 0.9f && x < mapSizeX - 2 && y < mapSizeY - 2)
                {
                    tileTypeMap[new Vector3(x, y, 0)] = grassyHills[0];

                    if (rollElevationChance > 0.90f && x < mapSizeX - 2 && y < mapSizeY - 2)
                    {
                        tileTypeMap[new Vector3(x, y + 1, 1)] = grassyHills[0];
                        tileTypeMap[new Vector3(x, y + 2, 1)] = grassyHills[0];
                        tileTypeMap[new Vector3(x + 1, y + 1, 1)] = grassyHills[0];
                        tileTypeMap[new Vector3(x + 1, y + 2, 1)] = grassyHills[0];

                        if (!tileTypeMap.ContainsKey(new Vector3(x, y, 1)))
                        {
                            tileTypeMap[new Vector3(x, y, 0.5f)] = grassyHills[3];
                        }
                        if (!tileTypeMap.ContainsKey(new Vector3(x + 1, y, 1)))
                        {
                            tileTypeMap[new Vector3(x + 1, y, 0.5f)] = grassyHills[3];
                        }

                        float rollMountainChance = Random.Range(0.0f, 1.0f);
                        if (rollMountainChance > 0.9f)
                        {
                            tileTypeMap[new Vector3(x, y + 2, 2)] = grassyHills[2];
                            tileTypeMap[new Vector3(x + 1, y + 2, 2)] = grassyHills[2];
                        }
                    }
                }
                else
                {
                    tileTypeMap[new Vector3(x, y, 0)] = grassyHills[0];
                }
                
                if (x < mapSizeX - 2 && y < mapSizeY - 2)
                {
                    if (rollTerrainChance > 0.9f && rollTerrainChance < 1.0f)
                    {
                        tileTypeMap[new Vector3(x, y, 0)] = grassyHills[1];
                        tileTypeMap[new Vector3(x + 1, y, 0)] = grassyHills[1];
                        tileTypeMap[new Vector3(x, y + 1, 0)] = grassyHills[1];
                        tileTypeMap[new Vector3(x + 1, y + 1, 0)] = grassyHills[1];
                    }
                }
            }
        }
    }
    
    /* GENERATE PROP DATA
     * This function is responsible for generating the random props on the map
     */
    public void GeneratePropData(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        propTypeMap = new Dictionary<Vector3, GrassyHillProps>();

        foreach (KeyValuePair<Vector3, TileSets.GrassyHillTiles> tile in tileTypeMap)
        {
            Vector3 tileLocation = tile.Key;

            if (!tileTypeMap.ContainsKey(new Vector3(tileLocation.x, tileLocation.y, tileLocation.z + 1))
                && !tileTypeMap.ContainsKey(new Vector3(tileLocation.x, tileLocation.y, tileLocation.z + 0.5f))
                && !propTypeMap.ContainsKey(new Vector3(tileLocation.x, tileLocation.y, tileLocation.z)))
            {
                float propChance = Random.Range(0.0f, 1.0f);

                if (propChance > 0.95f)
                {
                    float propTypeChance = Random.Range(0.0f, 1.0f);
                    
                    if (propTypeChance < 0.3)
                    {
                        propTypeMap[tileLocation] = grassyHillsProps[0];
                    }

                    if (propTypeChance > 0.3 && propTypeChance < 0.8)
                    {
                        propTypeMap[tileLocation] = grassyHillsProps[1];
                    }

                    if (propTypeChance > 0.8)
                    {
                        if (tileLocation.x < mapSizeX - 2
                            && propTypeMap.ContainsKey(new Vector3(tileLocation.x + 1, tileLocation.y, tileLocation.z))
                            && propTypeMap.ContainsKey(new Vector3(tileLocation.x + 2, tileLocation.y, tileLocation.z))
                            && !propTypeMap.ContainsKey(new Vector3(tileLocation.x + 2, tileLocation.y, tileLocation.z + 1))
                            && !propTypeMap.ContainsKey(new Vector3(tileLocation.x + 2, tileLocation.y, tileLocation.z + 1)))
                        {
                            propTypeMap[new Vector3(tileLocation.x, tileLocation.y, tileLocation.z)] = grassyHillsProps[2];
                            propTypeMap[new Vector3(tileLocation.x + 1, tileLocation.y, tileLocation.z)] = grassyHillsProps[3];
                            propTypeMap[new Vector3(tileLocation.x + 2, tileLocation.y, tileLocation.z)] = grassyHillsProps[4];
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
        tileMapDict = new Dictionary<Vector3, Tile>();

        propList = new List<Prop>();

        foreach (KeyValuePair<Vector3, TileSets.GrassyHillTiles> tileLocation in tileTypeMap)
        {
            /// Instantiating the tiles after setting their type based on the map graph
            TileSets.GrassyHillTiles type = tileLocation.Value;
            GameObject tile = Instantiate(type.tileVisualPrefab, new Vector3(tileLocation.Key.x, tileLocation.Key.y, tileLocation.Key.z), Quaternion.identity);
            tile.transform.parent = tiles.transform;

            /// Setting clickable tiles based on type
            Tile clickableTile = tile.GetComponent<Tile>();
            clickableTile.tileLocation = tileLocation.Key;

            tileMapDict[tileLocation.Key] = clickableTile;

            if (propTypeMap.ContainsKey(new Vector3(tileLocation.Key.x, tileLocation.Key.y, tileLocation.Key.z)))
            {
                clickableTile.hasProp = true;
            }
        }

        foreach (KeyValuePair<Vector3, GrassyHillProps> tileProp in propTypeMap)
        {
            /// Instantiating the props after setting their type based on the prop graph
            PropSets.GrassyHillProps type = tileProp.Value;
            GameObject prop = Instantiate(type.propVisualPrefab, new Vector3(tileProp.Key.x, tileProp.Key.y, tileProp.Key.z), Quaternion.identity);
            prop.transform.parent = props.transform;

            /// Setting props based on type
            Prop propInfo = prop.GetComponent<Prop>();
            
            propInfo.location = tileProp.Key;
            propInfo.isStructure = type.isStructure;
            propInfo.blocksTile = type.blocksTile;
            propInfo.coverType = type.coverType;

            propList.Add(propInfo);
        }
    }

    /* MAP GRAPH
     * This function creates nodes on all of the tiles and defines neighbors for pathfinding usage
     */
    public Dictionary<Vector3, Node> GenerateMapGraph(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        pathfindingGraph = new Dictionary<Vector3, Node>();

        /// Generate a node for each tile we have on the map
        foreach (KeyValuePair<Vector3, TileSets.GrassyHillTiles> tileLocation in tileTypeMap)
        {
            TileSets.GrassyHillTiles type = tileLocation.Value;
            Vector3 newNodeLocation = tileLocation.Key;
            
            pathfindingGraph[newNodeLocation] = new Node();
            pathfindingGraph[newNodeLocation].movementCost = type.movementCost;

            /// Setting unwalkable tiletypes to not be walkable on the node grid
            if (type.isWalkable == true)
            {
                pathfindingGraph[newNodeLocation].isWalkable = true;
            }
            else
            {
                pathfindingGraph[newNodeLocation].isWalkable = false;
            }

            /// Setting "underground" nodes to be unwalkable
            if (tileTypeMap.ContainsKey(new Vector3(newNodeLocation.x, newNodeLocation.y, newNodeLocation.z + 1)) || tileTypeMap.ContainsKey(new Vector3(newNodeLocation.x, newNodeLocation.y, newNodeLocation.z + 0.5f)))
            {
                pathfindingGraph[newNodeLocation].isWalkable = false;
            }

            pathfindingGraph[newNodeLocation].location = new Vector3(newNodeLocation.x, newNodeLocation.y, newNodeLocation.z);
        }

        foreach (KeyValuePair<Vector3, Node> node in pathfindingGraph)
        {
            Vector3 nodeLocation = node.Key;

            /// Adding all the neighbors that are on the same plane
            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x - 1, nodeLocation.y, nodeLocation.z)))
            {
                pathfindingGraph[nodeLocation].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x - 1, nodeLocation.y, nodeLocation.z)]);
            }

            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x + 1, nodeLocation.y, nodeLocation.z)))
            {
                pathfindingGraph[nodeLocation].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x + 1, nodeLocation.y, nodeLocation.z)]);
            }

            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x, nodeLocation.y - 1, nodeLocation.z)))
            {
                pathfindingGraph[nodeLocation].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y - 1, nodeLocation.z)]);
            }

            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x, nodeLocation.y + 1, nodeLocation.z)))
            {
                pathfindingGraph[nodeLocation].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y + 1, nodeLocation.z)]);
            }

            /// Adding "steps" between higher ground and the half tiles to the neighbors
            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x - 1, nodeLocation.y, nodeLocation.z - 0.5f)))
            {
                if (pathfindingGraph[new Vector3(nodeLocation.x - 1, nodeLocation.y, nodeLocation.z - 0.5f)].isWalkable)
                {
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x - 1, nodeLocation.y, nodeLocation.z - 0.5f)]);
                    pathfindingGraph[new Vector3(nodeLocation.x - 1, nodeLocation.y, nodeLocation.z - 0.5f)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)]);
                }
            }

            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x + 1, nodeLocation.y, nodeLocation.z - 0.5f)))
            {
                if (pathfindingGraph[new Vector3(nodeLocation.x + 1, nodeLocation.y, nodeLocation.z - 0.5f)].isWalkable)
                {
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x + 1, nodeLocation.y, nodeLocation.z - 0.5f)]);
                    pathfindingGraph[new Vector3(nodeLocation.x + 1, nodeLocation.y, nodeLocation.z - 0.5f)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)]);
                }
            }

            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x, nodeLocation.y - 1, nodeLocation.z - 0.5f)))
            {
                if (pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y - 1, nodeLocation.z - 0.5f)].isWalkable)
                {
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y - 1, nodeLocation.z - 0.5f)]);
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y - 1, nodeLocation.z - 0.5f)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)]);
                }
            }

            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x, nodeLocation.y + 1, nodeLocation.z - 0.5f)))
            {
                if (pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y + 1, nodeLocation.z - 0.5f)].isWalkable)
                {
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y + 1, nodeLocation.z - 0.5f)]);
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y + 1, nodeLocation.z - 0.5f)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)]);
                }
            }

            /// Adding "steps" between the half tiles to lower ground
            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x - 1, nodeLocation.y, nodeLocation.z - 0.5f)))
            {
                if (pathfindingGraph[new Vector3(nodeLocation.x - 1, nodeLocation.y, nodeLocation.z - 0.5f)].isWalkable)
                {
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x - 1, nodeLocation.y, nodeLocation.z - 0.5f)]);
                    pathfindingGraph[new Vector3(nodeLocation.x - 1, nodeLocation.y, nodeLocation.z - 0.5f)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)]);
                }
            }

            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x + 1, nodeLocation.y, nodeLocation.z - 0.5f)))
            {
                if (pathfindingGraph[new Vector3(nodeLocation.x + 1, nodeLocation.y, nodeLocation.z - 0.5f)].isWalkable)
                {
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x + 1, nodeLocation.y, nodeLocation.z - 0.5f)]);
                    pathfindingGraph[new Vector3(nodeLocation.x + 1, nodeLocation.y, nodeLocation.z - 0.5f)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)]);
                }
            }

            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x, nodeLocation.y - 1, nodeLocation.z - 0.5f)))
            {
                if (pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y - 1, nodeLocation.z - 0.5f)].isWalkable)
                {
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y - 1, nodeLocation.z - 0.5f)]);
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y - 1, nodeLocation.z - 0.5f)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)]);
                }
            }

            if (tileTypeMap.ContainsKey(new Vector3(nodeLocation.x, nodeLocation.y + 1, nodeLocation.z - 0.5f)))
            {
                if (pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y + 1, nodeLocation.z - 0.5f)].isWalkable)
                {
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y + 1, nodeLocation.z - 0.5f)]);
                    pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y + 1, nodeLocation.z - 0.5f)].neighbors.Add(pathfindingGraph[new Vector3(nodeLocation.x, nodeLocation.y, nodeLocation.z)]);
                }
            }
        }

        foreach (Prop prop in propList)
        {
            if (prop.blocksTile)
            {
                pathfindingGraph[new Vector3(prop.location.x, prop.location.y, prop.location.z)].isWalkable = false;
            }
        }

        GetWalkableNodes();

        return pathfindingGraph;
    }

    /* MAP LIGHTS
     * This function creates spotlights over all walkable tiles
     */
    public List<TileLight> GenerateMapLighting(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        lightMap = new Dictionary<Vector3, TileLight>();

        List<TileLight> tileLights = new List<TileLight>();

        foreach (Node node in walkableNodes)
        {
            Vector3 spawnAt = new Vector3(node.location.x, node.location.y, node.location.z + 5);
            GameObject activeTileLight = Resources.Load("Prefabs/Lights/TileLight") as GameObject;
            GameObject spawnedLight = Instantiate(activeTileLight, spawnAt, Quaternion.identity);

            spawnedLight.transform.Rotate(0, 180, 0);
            spawnedLight.transform.parent = tilelights.transform;

            TileLight tileLight = spawnedLight.GetComponent<TileLight>();
            tileLight.location = spawnAt;
            tileLight.enabledstatus = false;
            tileLight.lightobject = spawnedLight;

            lightMap[new Vector3(spawnAt.x, spawnAt.y, spawnAt.z)] = tileLight;
            tileLights.Add(tileLight);
        }

        /// Deactivate the lights
        foreach (TileLight tileLight in tileLights)
        {
            if (!tileLight.enabledstatus)
            {
                tileLight.lightobject.SetActive(false);
            }
        }

        return tileLights;
    }

    public void GenerateMapCover()
    {
        foreach (Node node in walkableNodes)
        {
            Vector3 tileLocation = node.location;

            tileMapDict[tileLocation].coverOnTile = new Dictionary<string, string>();

            if (tileMapDict.ContainsKey(new Vector3(tileLocation.x + 1, tileLocation.y, tileLocation.z)))
            {
                if (tileMapDict.ContainsKey(new Vector3(tileLocation.x + 1, tileLocation.y, tileLocation.z + 0.5f)))
                {
                    if (propTypeMap.ContainsKey(new Vector3(tileLocation.x + 1, tileLocation.y, tileLocation.z + 0.5f)))
                    {
                        tileMapDict[tileLocation].coverOnTile["West"] = "Full";
                    }
                    else
                    {
                        tileMapDict[tileLocation].coverOnTile["West"] = "Half";
                    }
                }
                else if (tileMapDict.ContainsKey(new Vector3(tileLocation.x + 1, tileLocation.y, tileLocation.z + 1)))
                {
                    tileMapDict[tileLocation].coverOnTile["West"] = "Full";
                }
                else if (propTypeMap.ContainsKey(new Vector3(tileLocation.x + 1, tileLocation.y, tileLocation.z)))
                {
                    if (propTypeMap[new Vector3(tileLocation.x + 1, tileLocation.y, tileLocation.z)].coverType == 1)
                    {
                        tileMapDict[tileLocation].coverOnTile["West"] = "Half";
                    }
                    if (propTypeMap[new Vector3(tileLocation.x + 1, tileLocation.y, tileLocation.z)].coverType == 2)
                    {
                        tileMapDict[tileLocation].coverOnTile["West"] = "Full";
                    }
                }
                else
                {
                    tileMapDict[tileLocation].coverOnTile["West"] = "None";
                }
            }

            if (tileMapDict.ContainsKey(new Vector3(tileLocation.x - 1, tileLocation.y, tileLocation.z)))
            {
                if (tileMapDict.ContainsKey(new Vector3(tileLocation.x - 1, tileLocation.y, tileLocation.z + 0.5f)))
                {
                    if (propTypeMap.ContainsKey(new Vector3(tileLocation.x - 1, tileLocation.y, tileLocation.z + 0.5f)))
                    {
                        tileMapDict[tileLocation].coverOnTile["East"] = "Full";
                    }
                    else
                    {
                        tileMapDict[tileLocation].coverOnTile["East"] = "Half";
                    }
                }
                else if (tileMapDict.ContainsKey(new Vector3(tileLocation.x - 1, tileLocation.y, tileLocation.z + 1)))
                {
                    tileMapDict[tileLocation].coverOnTile["East"] = "Full";
                }
                else if (propTypeMap.ContainsKey(new Vector3(tileLocation.x - 1, tileLocation.y, tileLocation.z)))
                {
                    if (propTypeMap[new Vector3(tileLocation.x - 1, tileLocation.y, tileLocation.z)].coverType == 1)
                    {
                        tileMapDict[tileLocation].coverOnTile["East"] = "Half";
                    }
                    if (propTypeMap[new Vector3(tileLocation.x - 1, tileLocation.y, tileLocation.z)].coverType == 2)
                    {
                        tileMapDict[tileLocation].coverOnTile["East"] = "Full";
                    }
                }
                else
                {
                    tileMapDict[tileLocation].coverOnTile["East"] = "None";
                }
            }

            if (tileMapDict.ContainsKey(new Vector3(tileLocation.x, tileLocation.y + 1, tileLocation.z)))
            {
                if (tileMapDict.ContainsKey(new Vector3(tileLocation.x, tileLocation.y + 1, tileLocation.z + 0.5f)))
                {
                    if (propTypeMap.ContainsKey(new Vector3(tileLocation.x, tileLocation.y + 1, tileLocation.z + 0.5f)))
                    {
                        tileMapDict[tileLocation].coverOnTile["North"] = "Full";
                    }
                    else
                    {
                        tileMapDict[tileLocation].coverOnTile["North"] = "Half";
                    }
                }
                else if (tileMapDict.ContainsKey(new Vector3(tileLocation.x, tileLocation.y + 1, tileLocation.z + 1)))
                {
                    tileMapDict[tileLocation].coverOnTile["North"] = "Full";
                }
                else if (propTypeMap.ContainsKey(new Vector3(tileLocation.x, tileLocation.y + 1, tileLocation.z)))
                {
                    if (propTypeMap[new Vector3(tileLocation.x, tileLocation.y + 1, tileLocation.z)].coverType == 1)
                    {
                        tileMapDict[tileLocation].coverOnTile["North"] = "Half";
                    }
                    if (propTypeMap[new Vector3(tileLocation.x, tileLocation.y + 1, tileLocation.z)].coverType == 2)
                    {
                        tileMapDict[tileLocation].coverOnTile["North"] = "Full";
                    }
                }
                else
                {
                    tileMapDict[tileLocation].coverOnTile["North"] = "None";
                }
            }

            if (tileMapDict.ContainsKey(new Vector3(tileLocation.x, tileLocation.y - 1, tileLocation.z)))
            {
                if (tileMapDict.ContainsKey(new Vector3(tileLocation.x, tileLocation.y - 1, tileLocation.z + 0.5f)))
                {
                    if (propTypeMap.ContainsKey(new Vector3(tileLocation.x, tileLocation.y - 1, tileLocation.z + 0.5f)))
                    {
                        tileMapDict[tileLocation].coverOnTile["South"] = "Full";
                    }
                    else
                    {
                        tileMapDict[tileLocation].coverOnTile["South"] = "Half";
                    }
                }
                else if (tileMapDict.ContainsKey(new Vector3(tileLocation.x, tileLocation.y - 1, tileLocation.z + 1)))
                {
                    tileMapDict[tileLocation].coverOnTile["South"] = "Full";
                }
                else if (propTypeMap.ContainsKey(new Vector3(tileLocation.x, tileLocation.y - 1, tileLocation.z)))
                {
                    if (propTypeMap[new Vector3(tileLocation.x, tileLocation.y - 1, tileLocation.z)].coverType == 1)
                    {
                        tileMapDict[tileLocation].coverOnTile["South"] = "Half";
                    }
                    if (propTypeMap[new Vector3(tileLocation.x, tileLocation.y - 1, tileLocation.z)].coverType == 2)
                    {
                        tileMapDict[tileLocation].coverOnTile["South"] = "Full";
                    }
                }
                else
                {
                    tileMapDict[tileLocation].coverOnTile["South"] = "None";
                }
            }
        }
    }

    /// Get walkable nodes from the node dictionary
    public void GetWalkableNodes()
    {
        walkableNodes = new List<Node>();

        foreach (KeyValuePair<Vector3, Node> tileLocation in pathfindingGraph)
        {
            if (pathfindingGraph[tileLocation.Key].isWalkable)
            {
                walkableNodes.Add(pathfindingGraph[tileLocation.Key]);
            }
        }
    }

    public Dictionary<Vector3, TileLight> ReturnLightGraph()
    {
        return lightMap;
    }

    public List<Node> ReturnWalkableNodeList()
    {
        return walkableNodes;
    }
}
