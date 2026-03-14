using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;  // Disable VSync to allow setting targetFrameRate
        Application.targetFrameRate = 60;  // Set the target frame rate to 30 FPS
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
