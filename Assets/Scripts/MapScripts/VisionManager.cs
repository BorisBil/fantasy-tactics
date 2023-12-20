using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionManager : MonoBehaviour
{
    public TileMap tileMap;
    public UnitManager unitManager;

    private List<Node> activeTiles;
    private List<Node> visionToRemove;
    private List<Node> visionToAdd;
    private List<Node> walkableNodes;

    private Node[,,] graph;
    private TileLight[,,] lightGraph;

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

            foreach (Node node in walkableNodes)
            {
                float distance = tileMap.GetDistance(
                    graph[(int)unit.unitPosition.x, (int)unit.unitPosition.y, (int)unit.unitPosition.z],
                    graph[node.location.x, node.location.y, node.location.z]);

                if (distance <= unit.visionRadius)
                {
                    Vector3 rayCastUnitCoords = new Vector3(unit.unitPosition.x, unit.unitPosition.y, unit.unitPosition.z + 1.00f);
                    Vector3 rayCastTileCoords = new Vector3(node.location.x, node.location.y, node.location.z + 1.00f);

                    Ray ray = new Ray(rayCastUnitCoords, rayCastTileCoords);
                    RaycastHit hitInfo;

                    if (!Physics.Raycast(ray, out hitInfo, distance) || (hitInfo.transform.parent == GameObject.Find("Prop")))
                    {
                        unit.visibleTiles.Add(graph[node.location.x, node.location.y, node.location.z]);
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
            lightGraph[node.location.x, node.location.y, node.location.z + 3].enabledstatus = true;
            lightGraph[node.location.x, node.location.y, node.location.z + 3].lightobject.SetActive(true);
        }
    }

    public void UpdateVision(Unit unit)
    {
        unit.visibleTiles.Clear();
        visionToAdd.Clear();
        visionToRemove.Clear();

        List<Node> temporaryList = new List<Node>();

        foreach (Node node in walkableNodes)
        {
            float distance = tileMap.GetDistance(
                graph[(int)unit.unitPosition.x, (int)unit.unitPosition.y, (int)unit.unitPosition.z],
                graph[node.location.x, node.location.y, node.location.z]);

            if (distance <= unit.visionRadius)
            {
                Vector3 rayCastUnitCoords = new Vector3(unit.unitPosition.x, unit.unitPosition.y, unit.unitPosition.z + 1.00f);
                Vector3 rayCastTileCoords = new Vector3(node.location.x, node.location.y, node.location.z + 1.00f);

                Ray ray = new Ray(rayCastUnitCoords, rayCastTileCoords);
                RaycastHit hitInfo;

                if (!Physics.Raycast(ray, out hitInfo, distance) || (hitInfo.transform.parent == GameObject.Find("Prop")))
                {
                    unit.visibleTiles.Add(graph[node.location.x, node.location.y, node.location.z]);
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

        foreach(Node node in visionToRemove)
        {
            lightGraph[node.location.x, node.location.y, node.location.z + 3].enabledstatus = false;
            lightGraph[node.location.x, node.location.y, node.location.z + 3].lightobject.SetActive(false);
        }

        foreach (Node node in visionToAdd)
        {
            lightGraph[node.location.x, node.location.y, node.location.z + 3].enabledstatus = true;
            lightGraph[node.location.x, node.location.y, node.location.z + 3].lightobject.SetActive(true);
        }
    }
}
