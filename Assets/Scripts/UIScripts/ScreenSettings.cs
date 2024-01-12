using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSettings
{
    public float screenWidth;
    public float screenHeight;

    public float screenWidthCut;
    public float screenHeightCut;

    public ScreenSettings() 
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    public void DetermineCameraCutAxis()
    {
        screenWidthCut = screenWidth / 8;
        screenHeightCut = screenHeight / 8;
    }
}
