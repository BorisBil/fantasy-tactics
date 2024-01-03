using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionManager : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileMap tileMap;
    public UnitManager unitManager;

    private List<Node> activeTiles;
    private List<Node> visionToRemove;
    private List<Node> visionToAdd;
    private List<Node> walkableNodes;

    private Dictionary<Vector3, Node> graph;
    private Dictionary<Vector3, TileLight> lightGraph;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    public void InstantiateVision()
    {
        activeTiles = new List<Node>();
        visionToRemove = new List<Node>();
        visionToAdd = new List<Node>();

        walkableNodes = tileMap.walkableNodes;

        graph = tileMap.graph;
        lightGraph = tileMap.lightsGraph;

        foreach (Unit unit in unitManager.playerUnits)
        {
            unit.visibleTiles = new List<Node>();

            Vector3 unitPosition = unit.unitPosition;

            foreach (Node node in walkableNodes)
            {
                Vector3 nodeLocation = node.location;

                float distance = tileMap.GetDistance(graph[unitPosition], graph[nodeLocation]);

                if (distance <= unit.visionRadius)
                {
                    Vector3 rayCastUnitCoords = new Vector3(unit.unitPosition.x, unit.unitPosition.y, unit.unitPosition.z + 2.00f);
                    Vector3 rayCastTileCoords = new Vector3(node.location.x, node.location.y, node.location.z + 2.00f);

                    Ray ray = new Ray(rayCastUnitCoords, rayCastTileCoords);
                    RaycastHit hitInfo;

                    if (!Physics.Raycast(ray, out hitInfo, distance) || (hitInfo.transform.parent == GameObject.Find("Props")))
                    {
                        unit.visibleTiles.Add(graph[nodeLocation]);
                    }
                }
            }
        }

        foreach (Unit unit in unitManager.playerUnits)
        {
            foreach (Node node in unit.visibleTiles)
            {
                if (!activeTiles.Contains(node))
                {
                    activeTiles.Add(node);
                }
            }
        }

        foreach (Node node in activeTiles)
        {
            lightGraph[new Vector3(node.location.x, node.location.y, node.location.z + 5)].enabledstatus = true;
            lightGraph[new Vector3(node.location.x, node.location.y, node.location.z + 5)].lightobject.SetActive(true);
        }
    }

    public void UpdateVision(Unit unit)
    {
        unit.visibleTiles.Clear();
        visionToAdd.Clear();
        visionToRemove.Clear();

        List<Node> temporaryList = new List<Node>();
        
        Vector3 unitPosition = unit.unitPosition;

        foreach (Node node in walkableNodes)
        {
            Vector3 nodeLocation = node.location;
            float distance = tileMap.GetDistance(graph[unitPosition], graph[nodeLocation]);

            if (distance <= unit.visionRadius)
            {
                Vector3 rayCastUnitCoords = new Vector3(unit.unitPosition.x, unit.unitPosition.y, unit.unitPosition.z + 2.00f);
                Vector3 rayCastTileCoords = new Vector3(node.location.x, node.location.y, node.location.z + 2.00f);

                Ray ray = new Ray(rayCastUnitCoords, rayCastTileCoords);
                RaycastHit hitInfo;

                if (!Physics.Raycast(ray, out hitInfo, distance) || (hitInfo.transform.parent == GameObject.Find("Props")))
                {
                    unit.visibleTiles.Add(graph[nodeLocation]);
                }
            }
        }

        foreach (Unit playerunit in unitManager.playerUnits)
        {
            foreach (Node node in playerunit.visibleTiles)
            {
                temporaryList.Add(node);
            }
        }

        foreach (Node node in activeTiles)
        {
            if (!temporaryList.Contains(node))
            {
                visionToRemove.Add(node);
            }
        }

        foreach (Node node in temporaryList)
        {
            if (!activeTiles.Contains(node))
            {
                visionToAdd.Add(node);
                activeTiles.Add(node);
            }
        }

        activeTiles = temporaryList;

        foreach(Node node in visionToRemove)
        {
            lightGraph[new Vector3(node.location.x, node.location.y, node.location.z + 5)].enabledstatus = false;
            lightGraph[new Vector3(node.location.x, node.location.y, node.location.z + 5)].lightobject.SetActive(false);
        }

        foreach (Node node in visionToAdd)
        {
            lightGraph[new Vector3(node.location.x, node.location.y, node.location.z + 5)].enabledstatus = true;
            lightGraph[new Vector3(node.location.x, node.location.y, node.location.z + 5)].lightobject.SetActive(true);
        }
    }
}
