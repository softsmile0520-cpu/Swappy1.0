using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenPanel : MonoBehaviour
{
    public void CloseSplashScreen()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
