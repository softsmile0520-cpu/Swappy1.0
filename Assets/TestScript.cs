using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject cube;
    public int cubeSize = 5;
    Vector3 PosNextLine = new Vector3();
    Vector3 PosSameLine = new Vector3();
    private void Start()
    {
        for (int i = 0; i < cubeSize; i++)
        {
            PosNextLine += new Vector3(0.5f,-1, 0);
            PosSameLine = PosNextLine;
            for (int j = 0; j < cubeSize; j++) 
            {
                if (j <= i)
                {
                    Instantiate(cube, PosSameLine, Quaternion.identity);
                    PosSameLine.x = PosSameLine.x - 1f;
                }
            }
        }
    }
}
