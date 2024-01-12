using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// 
/// PLAYER'S VIEW OF THE GAME
/// 

public class PlayerController : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileMap tileMap;

    public UnitManager unitManager;
    public GameLoopController gameLoopController;
    public ScreenSettings screenSettings;
    public CameraManager cameraManager;

    public GameObject mouseOver;
    public GameObject selectedUnit;
    public GameObject selectedEnemy;
    public GameObject cameraPivot;

    private Unit unit;
    private Tile tile;

    private float screenWidth;
    private float screenHeight;
    private float screenWidthCut;
    private float screenHeightCut;

    private float xCameraModifierLeftRight;
    private float xCameraModifierUpDown;
    private float yCameraModifierLeftRight;
    private float yCameraModifierUpDown;

    public enum CameraRotation { North, East, South, West };
    public CameraRotation desiredCameraRotationType;
    public CameraRotation currentCameraRotationType = CameraRotation.North;

    private Quaternion desiredCameraRotationAngle;

    private float cameraRotateSpeed = 100f;

    private List<Node> inRangeTiles;

    private List<Unit> toMoveQ;
    private List<Tile> toMoveTo;

    private bool attackMode;
    private bool moveMode;

    public bool isMoving;
    public bool transitionTurn;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    ///
    /// Set move mode on start, also create move queue
    /// 
    private void Start()
    {
        toMoveQ = new List<Unit>();
        toMoveTo = new List<Tile>();

        gameLoopController.SetUpLists();
        
        screenSettings = new ScreenSettings();
        screenSettings.DetermineCameraCutAxis();

        screenWidth = screenSettings.screenWidth;
        screenHeight = screenSettings.screenHeight;
        screenWidthCut = screenSettings.screenWidthCut;
        screenHeightCut = screenSettings.screenHeightCut;

        xCameraModifierLeftRight = 1;
        xCameraModifierUpDown = 1;
        yCameraModifierLeftRight = 1;
        yCameraModifierUpDown = 1;

        SetMoveMode();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObject = hitInfo.transform.parent.gameObject;
            
            if (hitObject == GameObject.Find("Tiles"))
            {
                hitObject = hitInfo.transform.gameObject;
            }
            
            mouseOver = hitObject;
        }

        if (moveMode)
        {
            /// CAMERA MOUSE MOVEMENT CONTROLS
            /// /// X AXIS
            if (Input.mousePosition.x > 0 && Input.mousePosition.x <= screenWidthCut)
            {
                Vector3 newCameraPosition = new Vector3(cameraPivot.transform.position.x + 0.1f * xCameraModifierLeftRight, cameraPivot.transform.position.y - 0.1f * yCameraModifierLeftRight, cameraPivot.transform.position.z);
                cameraPivot.transform.position = Vector3.MoveTowards(cameraPivot.transform.position, newCameraPosition, 0.05f);
            }
            if (Input.mousePosition.x > screenWidthCut && Input.mousePosition.x <= 2 * screenWidthCut)
            {
                Vector3 newCameraPosition = new Vector3(cameraPivot.transform.position.x + 0.05f * xCameraModifierLeftRight, cameraPivot.transform.position.y - 0.05f * yCameraModifierLeftRight, cameraPivot.transform.position.z);
                cameraPivot.transform.position = Vector3.MoveTowards(cameraPivot.transform.position, newCameraPosition, 0.025f);
            }
            if (Input.mousePosition.x > 6 * screenWidthCut && Input.mousePosition.x <= 7 * screenWidthCut)
            {
                Vector3 newCameraPosition = new Vector3(cameraPivot.transform.position.x - 0.05f * xCameraModifierLeftRight, cameraPivot.transform.position.y + 0.05f * yCameraModifierLeftRight, cameraPivot.transform.position.z);
                cameraPivot.transform.position = Vector3.MoveTowards(cameraPivot.transform.position, newCameraPosition, 0.025f);
            }
            if (Input.mousePosition.x > 7 * screenWidthCut && Input.mousePosition.x < screenWidth)
            {
                Vector3 newCameraPosition = new Vector3(cameraPivot.transform.position.x - 0.1f * xCameraModifierLeftRight, cameraPivot.transform.position.y + 0.1f * yCameraModifierLeftRight, cameraPivot.transform.position.z);
                cameraPivot.transform.position = Vector3.MoveTowards(cameraPivot.transform.position, newCameraPosition, 0.05f);
            }

            /// /// Y AXIS
            if (Input.mousePosition.y > 0 && Input.mousePosition.y <= screenHeightCut)
            {
                Vector3 newCameraPosition = new Vector3(cameraPivot.transform.position.x - 0.1f * xCameraModifierUpDown, cameraPivot.transform.position.y - 0.1f * yCameraModifierUpDown, cameraPivot.transform.position.z);
                cameraPivot.transform.position = Vector3.MoveTowards(cameraPivot.transform.position, newCameraPosition, 0.05f);
            }
            if (Input.mousePosition.y > screenHeightCut && Input.mousePosition.y <= 2 * screenHeightCut)
            {
                Vector3 newCameraPosition = new Vector3(cameraPivot.transform.position.x - 0.05f * xCameraModifierUpDown, cameraPivot.transform.position.y - 0.05f * yCameraModifierUpDown, cameraPivot.transform.position.z);
                cameraPivot.transform.position = Vector3.MoveTowards(cameraPivot.transform.position, newCameraPosition, 0.025f);
            }
            if (Input.mousePosition.y > 6 * screenHeightCut && Input.mousePosition.y <= 7 * screenHeightCut)
            {
                Vector3 newCameraPosition = new Vector3(cameraPivot.transform.position.x + 0.05f * xCameraModifierUpDown, cameraPivot.transform.position.y + 0.05f * yCameraModifierUpDown, cameraPivot.transform.position.z);
                cameraPivot.transform.position = Vector3.MoveTowards(cameraPivot.transform.position, newCameraPosition, 0.025f);
            }
            if (Input.mousePosition.y > 7 * screenHeightCut && Input.mousePosition.y < screenHeight)
            {
                Vector3 newCameraPosition = new Vector3(cameraPivot.transform.position.x + 0.1f * xCameraModifierUpDown, cameraPivot.transform.position.y + 0.1f * yCameraModifierUpDown, cameraPivot.transform.position.z);
                cameraPivot.transform.position = Vector3.MoveTowards(cameraPivot.transform.position, newCameraPosition, 0.05f);
            }

            if (currentCameraRotationType != desiredCameraRotationType)
            {
                var step = cameraRotateSpeed * Time.deltaTime;

                cameraPivot.transform.rotation = Quaternion.RotateTowards(cameraPivot.transform.rotation, desiredCameraRotationAngle, step);

                if (cameraPivot.transform.rotation == desiredCameraRotationAngle)
                {
                    cameraPivot.transform.rotation = desiredCameraRotationAngle;

                    currentCameraRotationType = desiredCameraRotationType;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (mouseOver.transform.parent.gameObject == GameObject.Find("PlayerUnits"))
                {
                    selectedUnit = mouseOver;
                    unit = selectedUnit.GetComponent<Unit>();

                    if (unit.attackableUnits.Count > 0 && unit.actionPoints >= 0)
                    {
                        gameLoopController.attackButton.ShowButton();
                    }
                    else
                    {
                        gameLoopController.attackButton.HideButton();
                    }

                    if (!unit.isMoving && unit.actionPoints > 1)
                    {
                        inRangeTiles = tileMap.TileRange(unit);

                        gameLoopController.ListPlayerAttackSelectable(unit);

                        if (unit.attackableUnits.Count > 0)
                        {
                            gameLoopController.attackButton.ShowButton();
                        }
                        else
                        {
                            gameLoopController.attackButton.HideButton();
                        }
                    }
                    else if (unit.actionPoints <= 0)
                    {
                        selectedUnit = null;
                        gameLoopController.attackButton.HideButton();
                    }
                }
                else if (!mouseOver.transform.parent.gameObject == GameObject.Find("Canvas"))
                {
                    selectedUnit = null;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (selectedUnit && selectedUnit.GetComponent<Unit>().actionPoints > 1 && !selectedUnit.GetComponent<Unit>().isMoving)
                {
                    if (mouseOver.transform.parent.gameObject == GameObject.Find("Tiles"))
                    {
                        tile = mouseOver.GetComponent<Tile>();
                        unit = selectedUnit.GetComponent<Unit>();

                        if (tileMap.graph[new Vector3(tile.tileLocation.x, tile.tileLocation.y, tile.tileLocation.z)].isWalkable)
                        {
                            bool contains = false;

                            for (int i = 0; i < inRangeTiles.Count; i++)
                            {
                                if (inRangeTiles[i].location == tile.tileLocation)
                                    contains = true;
                            }

                            if (contains && !unit.isMoving)
                            {
                                tileMap.UpdatePath(tile.tileLocation, unit);

                                toMoveQ.Add(unit);
                                toMoveTo.Add(tile);

                                gameLoopController.UpdateUnitCover(unit, null);

                                inRangeTiles = null;

                                tileMap.graph[new Vector3(unit.unitPosition.x, unit.unitPosition.y, unit.unitPosition.z)].isWalkable = true;
                                tileMap.graph[new Vector3(tile.tileLocation.x, tile.tileLocation.y, tile.tileLocation.z)].isWalkable = false;
                            }
                        }
                    }
                }
            }

            if (toMoveQ.Count > 0)
            {
                isMoving = true;
            }
            else if (toMoveQ.Count == 0)
            {
                isMoving = false;
            }

            if (isMoving)
            {
                gameLoopController.endTurnButton.HideButton();

                unit = toMoveQ[0];
                tile = toMoveTo[0];

                unit.isMoving = true;

                if (unit.unitPosition == tile.tileLocation)
                {
                    unit.currentPath = null;
                    unit.isMoving = false;
                    unit.actionPoints -= 1;

                    gameLoopController.UpdateUnitCover(unit, tile);

                    toMoveQ.RemoveAt(0);
                    toMoveTo.RemoveAt(0);

                    gameLoopController.ListPlayerVisibleUnits(unit);
                    gameLoopController.UpdatePlayerVisibleUnits();
                    gameLoopController.UpdatePlayerVision(unit);

                    gameLoopController.ListPlayerAttackSelectable(unit);

                    if (selectedUnit != null && selectedUnit.GetComponent<Unit>() == unit)
                    { 
                        if (unit.attackableUnits.Count > 0)
                        {
                            gameLoopController.attackButton.ShowButton();
                        }
                        else
                        {
                            gameLoopController.attackButton.HideButton();
                        }
                    }
                }

                if (unit.currentPath != null)
                {
                    var step = unit.unitSpeed * Time.deltaTime;
                    unit.transform.position = Vector3.MoveTowards(unit.transform.position, unit.currentPath[0].location, step);

                    if (unit.transform.position == unit.currentPath[0].location)
                    {
                        unit.unitPosition = unit.currentPath[0].location;
                        unit.transform.position = unit.unitPosition;
                        
                        gameLoopController.ListPlayerVisibleUnits(unit);
                        gameLoopController.UpdatePlayerVisibleUnits();
                        gameLoopController.UpdatePlayerVision(unit);

                        unit.currentPath.RemoveAt(0);
                    }
                }
            }
            else if (!isMoving && !transitionTurn)
            {
                gameLoopController.endTurnButton.ShowButton();
                gameLoopController.rotateCameraButton.ShowButton();
            }
        }

        if (attackMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (mouseOver.transform.parent.gameObject == GameObject.Find("EnemyUnits"))
                {
                    selectedEnemy = mouseOver;
                    Unit enemy = selectedEnemy.GetComponent<Unit>();
                    
                    if (unit.attackableUnits.Contains(enemy))
                    {
                        Unit unit = selectedUnit.GetComponent<Unit>();
                        gameLoopController.CalculateAttack(unit, enemy);
                        
                        selectedUnit = null;
                        selectedEnemy = null;

                        gameLoopController.attackButton.HideButton();
                        SetMoveMode();
                    }
                }
                else
                {
                    SetMoveMode();
                    selectedUnit = null;
                }
            }
        }
    }

    public void AddCameraRotation()
    {
        if (currentCameraRotationType == CameraRotation.North)
        {
            desiredCameraRotationType = CameraRotation.East;

            desiredCameraRotationAngle = Quaternion.Euler(35f, 33f, 54f);

            xCameraModifierUpDown = -1;
            yCameraModifierLeftRight = -1;
        }
        
        if (currentCameraRotationType == CameraRotation.East)
        {
            desiredCameraRotationType = CameraRotation.South;

            desiredCameraRotationAngle = Quaternion.Euler(-35f, 33f, 125f);

            xCameraModifierLeftRight = -1;

            xCameraModifierUpDown = -1;
            yCameraModifierUpDown = -1;
        }
        
        if (currentCameraRotationType == CameraRotation.South)
        {
            desiredCameraRotationType = CameraRotation.West;

            desiredCameraRotationAngle = Quaternion.Euler(-35f, -33f, 235f);

            xCameraModifierLeftRight = -1;
            yCameraModifierLeftRight = 1;
            
            xCameraModifierUpDown = 1;
            yCameraModifierUpDown = -1;
        }
        
        if (currentCameraRotationType == CameraRotation.West)
        {
            desiredCameraRotationType = CameraRotation.North;

            desiredCameraRotationAngle = Quaternion.Euler(35f, -33f, -54f);

            xCameraModifierLeftRight = 1;
            yCameraModifierLeftRight = 1;

            xCameraModifierUpDown = 1;
            yCameraModifierUpDown = 1;
        }
    }

    public void SetMoveMode()
    {
        attackMode = false;
        moveMode = true;
    }

    public void SetAttackMode()
    {
        attackMode = true;
        moveMode = false;
    }
}