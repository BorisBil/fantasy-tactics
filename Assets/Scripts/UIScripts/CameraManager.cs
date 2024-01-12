using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public PlayerController playerController;
    
    public GameObject cameraPivot;

    public void ChangeCameraRotation()
    {
        playerController.AddCameraRotation();
    }
}
