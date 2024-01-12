using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class RotateCamera : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    Button rotateCamera;
    
    public GameLoopController gameLoopController;
    public PlayerController playerController;
    public CameraManager cameraManager;

    public GameObject CameraPivot;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    private void Start()
    {
        rotateCamera = GetComponent<Button>();

        rotateCamera.gameObject.SetActive(true);

        rotateCamera.onClick.AddListener(() => RotateTheCamera());
    }

    public void RotateTheCamera()
    {
        cameraManager.ChangeCameraRotation();
    }

    /// Show the button
    public void ShowButton()
    {
        rotateCamera.gameObject.SetActive(true);
    }

    /// Hide the button
    public void HideButton()
    {
        rotateCamera.gameObject.SetActive(false);
    }
}
